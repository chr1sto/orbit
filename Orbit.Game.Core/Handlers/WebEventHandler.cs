using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orbit.Domain.Core.Events;
using Orbit.Domain.Game.Events;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Services;
using System;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Handlers
{
    public class WebEventHandler : IWebEventHandler
    {
        private readonly IWebEventService _webEventService;
        private readonly IGameAccountService _gameAccountService;
        private readonly ILogger<WebEventHandler> _logger;

        public WebEventHandler(IWebEventService webEventService,IGameAccountService gameAccountService, ILogger<WebEventHandler> logger)
        {
            _webEventService = webEventService;
            _gameAccountService = gameAccountService;
            _logger = logger;
        }

        public async Task<bool> PollAndHandleEvents()
        {
            var events = await _webEventService.GetUnhandled();
            _logger.LogInformation("Found {0} unhandled Events.", events.Count);
            foreach(var @event in events)
            {
                try
                {
                    this.HandleEvent(@event);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning("Exception while \"{0}\" with Id {1}\nException:\n{2}", @event.MessageType, @event.Id.ToString(), ex.Message);
                }
            }

            return true;
        }
        private async void HandleEvent(StoredEvent @event)
        {
            bool handled = false;
            switch(@event.MessageType)
            {
                case "GameAccountCreatedEvent":
                    handled = await this.HandleGameAccountCreatedEvent(@event.Data);
                    break;
                default:
                    _logger.LogWarning("Unrecognized Event Type \"{0}\" in queue",@event.MessageType);
                    break;
            }

            if(handled)
            {
                if(@event.Id != null)
                {
                    await _webEventService.SetHandled((Guid)@event.Id);
                }
            }
        }

        private async Task<bool> HandleGameAccountCreatedEvent(string serializedData)
        {
            var deserializedData = JsonConvert.DeserializeObject<GameAccountCreatedEvent>(serializedData);
            var result = await _gameAccountService.CreateAccount(deserializedData.Account);
            return result;
        }
    }
}
