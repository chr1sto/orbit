using Orbit.Domain.ServiceStatus.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Commands
{
    public class RemoveServiceStatusCommand : ServiceStatusCommand
    {
        public RemoveServiceStatusCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveServiceStatusValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
