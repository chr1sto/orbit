using FluentValidation;
using Orbit.Domain.News.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Validations
{
    public abstract class NewsPostValidation<T> : AbstractValidator<T> where T : NewsPostCommand
    {
        protected void ValidateCaption()
        {
            RuleFor(c => c.Caption)
                .NotEmpty().WithMessage("You need to provide a Catpion/Title!");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
