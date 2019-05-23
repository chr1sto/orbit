using FluentValidation;
using Orbit.Domain.Generic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Validations
{
    public abstract class GenericObjectValidation<T> : AbstractValidator<T> where T : GenericObjectCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateType()
        {
            RuleFor(c => c.Type)
                .NotEmpty().WithMessage("A Type hast to be provided for the GenericObject!");
        }
    }
}
