using FluentValidation;
using Orbit.Domain.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Validations
{
    public abstract class TransactionValidation<T> : AbstractValidator<T> where T : TransactionCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateReason()
        {
            RuleFor(c => c.Reason)
                .NotEmpty()
                .WithMessage("You need to provide a Reason!");
        }

        protected void ValidateUserId()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
