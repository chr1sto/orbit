using Orbit.Domain.GameAccount.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Validations
{
    public class CreateGameAccountValidation : GameAccountValidation<CreateGameAccountCommand>
    {
        public CreateGameAccountValidation()
        {
            ValidateAlias();
            ValidateId();
        }
    }
}
