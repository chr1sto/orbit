using Orbit.Domain.Core.Commands;
using Orbit.Domain.Game.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Commands
{
    public abstract class ServiceStatusCommand : Command
    {
        public Guid Id { get; protected set; }
        public EServiceState State { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        public string Service { get; protected set; }
    }
}
