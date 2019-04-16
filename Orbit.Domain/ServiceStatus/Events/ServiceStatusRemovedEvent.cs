using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Events
{
    public class ServiceStatusRemovedEvent : Event
    {
        public ServiceStatusRemovedEvent(Guid id, Guid userID, string account, string alias)
        {
            Id = id;
            UserID = userID;
            Account = account;
            Alias = alias;
        }

        public Guid Id { get; set; }
        public EServiceState State { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Service { get; set; }
    }
}
