using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Core.Events
{
    public class StoredEvent : Event
    {
        public StoredEvent(Event theEvent, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = theEvent.AggregateId;
            MessageType = theEvent.MessageType;
            Data = data;
            User = user;
            Handled = false;
        }

        // EF Constructor
        public StoredEvent() { }

        public Guid Id { get; set; }
        public string Data { get; set; }
        public string User { get; set; }
        public bool Handled { get; set; }
        public DateTime HandledTimeStamp { get; set; }
    }
}
