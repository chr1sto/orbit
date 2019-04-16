using Orbit.Domain.Core.Events;
using Orbit.Domain.Game.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Events
{
    public class ServiceStatusUpdatedEvent : Event
    {
        public ServiceStatusUpdatedEvent(Guid id, EServiceState state, DateTime timeStamp, string service)
        {
            Id = id;
            State = state;
            TimeStamp = timeStamp;
            Service = service;
        }

        public Guid Id { get; set; }
        public EServiceState State { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Service { get; set; }
    }
}
