using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Commands
{
    public abstract class GameAccountCommand : Command
    {
        public Guid ID { get; protected set; }
        public Guid UserID { get; protected set; }
        public string Account { get; protected set; }
        public string Alias { get; protected set; }
        public string Server { get; protected set; }
    }
}
