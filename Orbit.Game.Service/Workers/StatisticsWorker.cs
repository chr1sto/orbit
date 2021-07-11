using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Service.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service.Workers
{
    public class StatisticsWorker : BackgroundService
    {
        private readonly ILogger<StatisticsWorker> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IStatisticsService _statisticsService;
        private readonly StatisticsClient _client;

        public StatisticsWorker(ILogger<StatisticsWorker> logger, IStatisticsService statisticsService,HttpClient httpClient, AuthenticationService authenticationService, IConfiguration configuration)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _statisticsService = statisticsService;
            _client = new StatisticsClient(httpClient);
            _client.BaseUrl = configuration["BASE_API_PATH"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _authenticationService.Authenticate();
                    //Update Statistics Entry For Online Characters
                    var channels = _configuration.GetSection("Channels").Get<int[]>(); ;
                    foreach(var item in channels)
                    {
                        var playerCount = _statisticsService.GetPlayerCount(item);
                        await _client.StatisticsPostAsync(new StatisticsEntryViewModel()
                        {
                            Start = DateTime.Now,
                            End = DateTime.Now,
                            Id = Guid.NewGuid(),
                            StatGroup = "PLAYER_COUNT",
                            StatName = item.ToString(),
                            Value = playerCount.ToString(),
                            @ValueType = "number"
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Unable to update service states:\nException:\n{0}", ex.Message);
                }
                int.TryParse(_configuration["Intervals:Statistics"] ?? "120000", out int delay);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
