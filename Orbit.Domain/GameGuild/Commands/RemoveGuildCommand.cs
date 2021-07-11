using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameGuild.Commands
{
    public class RemoveGuildCommand : GuildCommand
    {
        public RemoveGuildCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
