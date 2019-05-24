using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Statistics
{
    public class StatisticsEntry : Entity
    {
        public StatisticsEntry(Guid id, DateTime start, DateTime end, string statGroup, string statName, string valueType, string value)
        {
            Id = id;
            Start = start;
            End = end;
            StatGroup = statGroup;
            StatName = statName;
            ValueType = valueType;
            Value = value;
        }

        protected StatisticsEntry() { }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public string StatGroup { get; private set; }
        public string StatName { get; private set; }
        public string ValueType { get; private set; }
        public string Value { get; private set; }
    }
}
