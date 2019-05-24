using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Statistics.Commands
{
    public abstract class StatisticsEntryCommand : Command
    {
        public StatisticsEntryCommand(DateTime start, DateTime end, string statGroup, string statName, string valueType, string value)
        {
            Id = Guid.NewGuid();
            Start = start;
            End = end;
            StatGroup = statGroup;
            StatName = statName;
            ValueType = valueType;
            Value = value;
        }

        public Guid Id { get; protected set; }
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        public string StatGroup { get; protected set; }
        public string StatName { get; protected set; }
        public string ValueType { get; protected set; }
        public string Value { get; protected set; }
    }
}
