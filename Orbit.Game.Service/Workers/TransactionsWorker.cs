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
    public class TransactionsWorker : BackgroundService
    {

        private readonly ILogger<TransactionsWorker> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly IProcessTransactionsService _processTransactionsService;
        private readonly IConfiguration _configuration;

        public TransactionsWorker(ILogger<TransactionsWorker> logger, AuthenticationService authenticationService, IProcessTransactionsService processTransactionsService, IConfiguration configuration)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _processTransactionsService = processTransactionsService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _authenticationService.Authenticate();
                    await _processTransactionsService.Process();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Unable to process transactions:\nException:\n{0}", ex.Message);
                }
                int.TryParse(_configuration["Intervals:Character"] ?? "1000", out int delay);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
