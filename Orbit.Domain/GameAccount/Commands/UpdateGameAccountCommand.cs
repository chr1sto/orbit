using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Commands
{
    public class UpdateGameAccountCommand : GameAccountCommand
    {
        public UpdateGameAccountCommand(Guid iD, string account, string alias)
        {
            ID = iD;
            Account = account;
            Alias = alias;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
