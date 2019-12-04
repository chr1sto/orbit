using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.PlayerLog.Commands
{
    public abstract class PlayerLogCommand : Command
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        public string Info { get; protected set; }
        public string IpAddress { get; protected set; }
    }
}
