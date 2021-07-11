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
    public class CharacterWorker : BackgroundService
    {
        private readonly ILogger<CharacterWorker> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly IGameCharacterService _gameCharacterService;
        private readonly IConfiguration _configuration;

        public CharacterWorker(ILogger<CharacterWorker> logger, AuthenticationService authenticationService, IGameCharacterService gameCharacterService, IConfiguration configuration)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _gameCharacterService = gameCharacterService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _authenticationService.Authenticate();
                    //TODO Async!!
                    _gameCharacterService.UpdateAll();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Unable to update characters:\nException:\n{0}", ex.Message);
                }
                int.TryParse(_configuration["Intervals:Character"] ?? "1000" ,out int delay);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
