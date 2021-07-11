using Orbit.Domain.GameAccount.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Validations
{
    public class UpdateGameAccountValidation : GameAccountValidation<UpdateGameAccountCommand>
    {
        public UpdateGameAccountValidation()
        {
            ValidateAlias();
            ValidateId();
        }
    }
}
