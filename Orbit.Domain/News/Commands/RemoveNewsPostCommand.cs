using Orbit.Domain.News.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Commands
{
    public class RemoveNewsPostCommand : NewsPostCommand
    {
        public RemoveNewsPostCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveNewsPostValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
