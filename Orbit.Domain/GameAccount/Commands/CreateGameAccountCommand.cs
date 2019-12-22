using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameAccount.Commands
{
    public class CreateGameAccountCommand : GameAccountCommand
    {
        public CreateGameAccountCommand(Guid id, string account, string alias, string server)
        {
            ID = id;
            Account = account;
            Alias = alias;
            Server = server;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
