using Orbit.Domain.Statistics.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Statistics.Commands
{
    public class CreateStatisticsEntryCommand : StatisticsEntryCommand
    {
        public CreateStatisticsEntryCommand(DateTime start, DateTime end, string statGroup, string statName, string valueType, string value)
        {
            Id = Guid.NewGuid();
            Start = start;
            End = end;
            StatGroup = statGroup;
            StatName = statName;
            ValueType = valueType;
            Value = value;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateStatisticsEntryValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
