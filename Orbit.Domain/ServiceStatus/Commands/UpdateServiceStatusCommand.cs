using Orbit.Domain.Game.Enums;
using Orbit.Domain.ServiceStatus.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Commands
{
    public class UpdateServiceStatusCommand : ServiceStatusCommand
    {
        public UpdateServiceStatusCommand(Guid id,string service, EServiceState state)
        {
            Id = id;
            Service = service;
            State = state;
            TimeStamp = DateTime.Now;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateServiceStatusValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
