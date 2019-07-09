using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Statistics.Commands
{
    public abstract class StatisticsEntryCommand : Command
    {
        public Guid Id { get; protected set; }
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        public string StatGroup { get; protected set; }
        public string StatName { get; protected set; }
        public string ValueType { get; protected set; }
        public string Value { get; protected set; }
    }
}
