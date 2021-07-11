using Orbit.Domain.Game.Enums;
using Orbit.Domain.ServiceStatus.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Commands
{
    public class CreateServiceStatusCommand : ServiceStatusCommand
    {
        public CreateServiceStatusCommand(string service, EServiceState state)
        {
            Id = Guid.NewGuid();
            Service = service;
            State = state;
            TimeStamp = DateTime.Now;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateServiceStatusValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
