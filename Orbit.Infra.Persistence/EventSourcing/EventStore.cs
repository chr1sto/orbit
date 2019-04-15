using System;
using Newtonsoft.Json;
using Orbit.Domain.Core.Events;
using Orbit.Domain.Core.Interfaces;
using Orbit.Infra.Persistence.Repository.EventSourcing;

namespace Orbit.Infra.Persistence.EventSourcing
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public EventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void Handle(Guid id)
        {
            _eventStoreRepository.Handle(id);
        }

        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(theEvent);

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                _user.Name);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}
