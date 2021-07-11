using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.Persistence.Repository.EventSourcing
{
    public interface IEventStoreRepository : IDisposable
    {
        void Store(StoredEvent theEvent);
        IList<StoredEvent> All(Guid aggregateId);
        IList<StoredEvent> All(params string[] actions);
        void Handle(Guid id);
    }
}
