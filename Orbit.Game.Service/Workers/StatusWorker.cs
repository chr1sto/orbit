using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service.Workers
{
    public class StatusWorker : BackgroundService
    {
        private readonly ILogger<StatusWorker> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly IServiceStatusService _serviceStatusService;
        private readonly IConfiguration _configuration;

        public StatusWorker(ILogger<StatusWorker> logger, AuthenticationService authenticationService, IServiceStatusService serviceStatusService, IConfiguration configuration)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _serviceStatusService = serviceStatusService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _authenticationService.Authenticate();
                    //TODO: async
                    _serviceStatusService.Update();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Unable to update service states:\nException:\n{0}", ex.Message);
                }
                int.TryParse(_configuration["Intervals:Character"] ?? "1000", out int delay);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
