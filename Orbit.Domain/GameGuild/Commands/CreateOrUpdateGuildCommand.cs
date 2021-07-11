using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameGuild.Commands
{
    public class CreateOrUpdateGuildCommand : GuildCommand
    {
        public CreateOrUpdateGuildCommand(string name, string guildId, string playerID, int guildScore, int averageGearScore, int totalGearScore, int level, DateTime createdOn)
        {
            Name = name;
            GuildId = guildId;
            PlayerId = playerID;
            GuildScore = guildScore;
            AverageGearScore = averageGearScore;
            TotalGearScore = totalGearScore;
            Level = level;
            CreatedOn = createdOn;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
