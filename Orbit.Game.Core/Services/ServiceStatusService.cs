using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orbit.Domain.Game.Enums;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Orbit.Game.Core.Services
{
    public class ServiceStatusService : IServiceStatusService
    {
        private readonly ILogger<ServiceStatusService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly MonitorProcessInfo[] _monitorProcessInfos;

        public ServiceStatusService(ILogger<ServiceStatusService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;


            var section = configuration.GetSection("ProcessesToMonitor");
            List<MonitorProcessInfo> processInfos = new List<MonitorProcessInfo>();
            foreach(IConfigurationSection item in section.GetChildren())
            {
                processInfos.Add(new MonitorProcessInfo()
                {
                    ProcessName = item.GetValue<string>("ProcessName"),
                    ServiceName = item.GetValue<string>("ServiceName")
            });
            }

            _monitorProcessInfos = processInfos.ToArray();
        }

        public void Update()
        {

            var client = new ServiceStatusClient(_httpClient);
            client.BaseUrl = _configuration["BASE_API_PATH"];

            client.ServiceStatusPostAsync(new ServiceStatusViewModel() {
                Service = "GameService",
                State = (int)EServiceState.Online
            }).Wait();

            var filePath = _configuration["ACCOUNT_INI_FILE_PATH"];

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.Contains("Maintenance") && line.Contains("1"))
                    {
                        foreach (var monitorProcessInfo in _monitorProcessInfos)
                        {
                            client.ServiceStatusPostAsync(new ServiceStatusViewModel()
                            {
                                Service = monitorProcessInfo.ServiceName,
                                State = (int)EServiceState.Maintenance
                            }).Wait();
                        }

                        return;
                    }
                }
            }


            foreach (var monitorProcessInfo in _monitorProcessInfos)
            {
                EServiceState state;
                var processes = Process.GetProcessesByName(monitorProcessInfo.ProcessName);
                if(processes == null || processes.Length == 0)
                {
                    state = EServiceState.Offline;
                }
                else
                {
                    state = EServiceState.Online;
                    if(processes.Length > 1)
                    {
                        _logger.LogWarning("Found more than one Process with the Name \"{0}\"", monitorProcessInfo.ProcessName);
                    }
                }

                client.ServiceStatusPostAsync(new ServiceStatusViewModel() {
                    Service = monitorProcessInfo.ServiceName,
                    State = (int)state
                }).Wait();
            }
        }
    }
}
