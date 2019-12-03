using Orbit.Domain.Core.Models;
using Orbit.Domain.GameCharacter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameGuild
{
    public class Guild : Entity
    {
        public Guild(Guid id,string name, string guildId, int guildScore, int averageGearScore, int totalGearScore, int level, DateTime createdOn)
        {
            Id = id;
            Name = name;
            GuildId = guildId;
            GuildScore = guildScore;
            AverageGearScore = averageGearScore;
            TotalGearScore = totalGearScore;
            Level = level;
            CreatedOn = createdOn;
        }

        protected Guild() { }

        public string Name { get; set; }
        public string GuildId { get; set; }
        public virtual GameCharacter.Character Leader { get; set; }
        public int GuildScore { get; set; }
        public int AverageGearScore { get; set; }
        public int TotalGearScore { get; set; }
        public int Level { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
