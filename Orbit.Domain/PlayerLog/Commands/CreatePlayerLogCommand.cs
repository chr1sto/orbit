using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.PlayerLog.Commands
{
    public class CreatePlayerLogCommand : PlayerLogCommand
    {
        public CreatePlayerLogCommand(string info)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            Info = info;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
