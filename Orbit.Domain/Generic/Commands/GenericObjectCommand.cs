using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Commands
{
    public abstract class GenericObjectCommand : Command
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public string Type { get; protected set; }
        public string ValueType { get; protected set; }
        public string Value { get; protected set; }
        public bool Visible { get; protected set; }
    }
}
