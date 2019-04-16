using FluentValidation;
using Orbit.Domain.ServiceStatus.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Validations
{
    public abstract class ServiceStatusValidation<T> : AbstractValidator<T> where T : ServiceStatusCommand
    {
        protected void ValidateService()
        {
            RuleFor(c => c.Service)
                .NotEmpty().WithMessage("A Service Name needs to be provided!");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
