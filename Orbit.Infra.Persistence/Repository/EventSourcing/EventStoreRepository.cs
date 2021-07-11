using Orbit.Domain.Core.Events;
using Orbit.Infra.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Orbit.Infra.Persistence.Repository.EventSourcing
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreContext _context;

        public EventStoreRepository(EventStoreContext context)
        {
            _context = context;
        }

        public IList<StoredEvent> All(Guid aggregateId)
        {
            return (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToList();
        }

        public IList<StoredEvent> All(params string[] actions)
        {
            return (from e in _context.StoredEvent where (actions.Contains(e.MessageType) && e.Handled == false) select e).ToList();
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Handle(Guid id)
        {
            var result = _context.StoredEvent.Find(id);
            if(result != null)
            {
                result.Handled = true;
                result.HandledTimeStamp = DateTime.Now;
                _context.StoredEvent.Update(result);
                _context.SaveChanges();
            }
        }
    }
}
