using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter.Commands
{
    public abstract class CharacterCommand : Command
    {
        public Guid Id { get; protected set; }
        public DateTime UpdatedOn { get; protected set; }
        public Guid UpdateId { get; protected set; }
        public bool IsStaff { get; protected set; }
        public string PlayerId { get; protected set; }
        public string Account { get; protected set; }
        public string Name { get; protected set; }
        public string Class { get; protected set; }
        public int GearScore { get; protected set; }
        public int Level { get; protected set; }
        public int PlayTime { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public int Strength { get; protected set; }
        public int Dexterity { get; protected set; }
        public int Stamina { get; protected set; }
        public int Intelligence { get; protected set; }
        public long Perin { get; protected set; }
        public long Penya { get; protected set; }
        public long RedChips { get; protected set; }
        public long EuphresiaCoins { get; protected set; }
        public long VotePoints { get; protected set; }
        public long DonateCoins { get; protected set; }
        public int BossKills { get; protected set; }
        public bool IsDeleted { get; protected set; }

    }
}
