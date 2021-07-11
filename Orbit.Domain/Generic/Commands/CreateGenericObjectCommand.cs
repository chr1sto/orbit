using Orbit.Domain.Generic.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Commands
{
    public class CreateGenericObjectCommand : GenericObjectCommand
    {
        public CreateGenericObjectCommand(DateTime createdOn, string type, string valueType, string value, bool visible)
        {
            CreatedOn = createdOn;
            Type = type;
            ValueType = valueType;
            Value = value;
            Visible = visible;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateGenericObjectValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
