using Orbit.Domain.Generic.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Commands
{
    public class UpdateGenericObjectCommand : GenericObjectCommand
    {
        public UpdateGenericObjectCommand(Guid id, DateTime createdOn, string type, string valueType, string value, bool visible)
        {
            Id = id;
            CreatedOn = createdOn;
            Type = type;
            ValueType = valueType;
            Value = value;
            Visible = visible;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateGenericObjectValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
