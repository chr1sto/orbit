using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Commands
{
    public class CreateGameAccountCommand : GameAccountCommand
    {
        public CreateGameAccountCommand(Guid id, string account, string alias)
        {
            ID = id;
            Account = account;
            Alias = alias;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
