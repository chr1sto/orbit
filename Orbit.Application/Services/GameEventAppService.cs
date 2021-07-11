using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Events;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Services
{
    public class GameEventAppService : IGameEventAppService
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _bus;
        private readonly string[] _actionsToHandle =
        {
            "GameAccountCreatedEvent",
            "GameAccountUpdatedEvent"
        };

        public GameEventAppService(IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public IList<StoredEvent> GetUnhandled()
        {
            var events = _eventStoreRepository.All(_actionsToHandle);
            return events;
        }

        public void Handle(Guid id)
        {
            _eventStoreRepository.Handle(id);
        }
    }
}
