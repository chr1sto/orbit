using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orbit.Domain.Game.Events;
using Orbit.Game.Core;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Misc;
using Orbit.Game.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service.Workers
{
    public class WebEventWorker : BackgroundService
    {

        private readonly IWebEventService _webEventService;
        private readonly IGameAccountService _gameAccountService;
        private readonly ILogger<WebEventWorker> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;

        public WebEventWorker(IWebEventService webEventService, IGameAccountService gameAccountService, ILogger<WebEventWorker> logger, AuthenticationService authenticationService, IConfiguration configuration)
        {
            _webEventService = webEventService;
            _gameAccountService = gameAccountService;
            _logger = logger;
            _authenticationService = authenticationService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _authenticationService.Authenticate();
                    await this.PollAndHandleEvents();
                }
                catch(Exception ex)
                {
                    _logger.LogCritical("Unable to poll events:\nException:\n{0}", ex.Message);
                }
                int.TryParse(_configuration["Intervals:Character"] ?? "1000", out int delay);
                await Task.Delay(delay, stoppingToken);
            }
        }

        private async Task<bool> PollAndHandleEvents()
        {
            var events = await _webEventService.GetUnhandled();
            _logger.LogInformation("Found {0} unhandled Events.", events.Count);
            foreach (var @event in events)
            {
                try
                {
                    this.HandleEvent(@event);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Exception while \"{0}\" with Id {1}\nException:\n{2}", @event.MessageType, @event.Id.ToString(), ex.Message);
                }
            }

            return true;
        }

        private async void HandleEvent(StoredEvent @event)
        {
            bool handled = false;

            EventWrapper data = JsonConvert.DeserializeObject<EventWrapper>(@event.Data);

            if (data != null)
            {
                switch (data.MessageType)
                {
                    case "GameAccountCreatedEvent":
                        handled = await this.HandleGameAccountCreatedEvent(@event.Data);
                        break;
                    default:
                        _logger.LogWarning("Unrecognized Event Type \"{0}\" in queue", @event.MessageType);
                        break;
                }

                if (handled)
                {
                    if (@event.Id != null)
                    {
                        await _webEventService.SetHandled((Guid)@event.Id);
                    }
                }
            }
        }

        private async Task<bool> HandleGameAccountCreatedEvent(string serializedData)
        {
            var deserializedData = JsonConvert.DeserializeObject<GameAccountCreatedEvent>(serializedData);
            var result = await _gameAccountService.CreateAccount(deserializedData.Account, deserializedData.UserID.ToString());
            return result;
        }
    }
}
