using System;
using System.Collections.Generic;
using System.Text;
using Orbit.Domain.Core.Commands;
using Orbit.Domain.GameCharacter;

namespace Orbit.Domain.GameGuild.Commands
{
    public abstract class GuildCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string GuildId { get; protected set; }
        public string PlayerId { get; protected set; }
        public int GuildScore { get; protected set; }
        public int AverageGearScore { get; protected set; }
        public int TotalGearScore { get; protected set; }
        public int Level { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
    }
}
