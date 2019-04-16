using Orbit.Domain.Core.Models;
using Orbit.Domain.Game.Enums;
using System;

namespace Orbit.Domain.Game.Models
{
    public class ServiceStatus : Entity
    {
        public ServiceStatus(Guid id,EServiceState state, DateTime timeStamp, string service)
        {
            Id = id;
            State = state;
            TimeStamp = timeStamp;
            Service = service;
        }
        public EServiceState State { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Service { get; set; }
    }
}
