using FluentValidation;
using Orbit.Domain.Statistics.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Statistics.Validations
{
    public class CreateStatisticsEntryValidation : AbstractValidator<CreateStatisticsEntryCommand>
    {
        public CreateStatisticsEntryValidation()
        {
            ValidateStatGroup();
            ValidateStatName();
            ValidateValueType();
            ValidateValue();
        }

        private void ValidateStatGroup()
        {
            RuleFor(c => c.StatGroup)
                .NotEmpty().WithMessage("A StatGroup hast to be provided");
        }

        private void ValidateStatName()
        {
            RuleFor(c => c.StatName)
                .NotEmpty().WithMessage("A StatName hast to be provided");
        }

        private void ValidateValueType()
        {
            RuleFor(c => c.ValueType)
                .NotEmpty().WithMessage("A ValueType hast to be provided");
        }

        private void ValidateValue()
        {
            RuleFor(c => c.Value)
                .NotEmpty().WithMessage("A Value hast to be provided");
        }
    }
}
