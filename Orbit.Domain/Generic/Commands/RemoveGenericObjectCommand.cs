using Orbit.Domain.Generic.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Commands
{
    public class RemoveGenericObjectCommand : GenericObjectCommand
    {
        public RemoveGenericObjectCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveGenericObjectValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
