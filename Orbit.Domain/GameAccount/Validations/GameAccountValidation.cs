using FluentValidation;
using Orbit.Domain.GameAccount.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Validations
{
    public abstract class GameAccountValidation<T> : AbstractValidator<T> where T : GameAccountCommand
    {
        protected void ValidateAlias()
        {
            RuleFor(c => c.Alias)
                .NotEmpty().WithMessage("You need to provide a Catpion/Title!");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.ID)
                .NotEqual(Guid.Empty);
        }
    }
}
