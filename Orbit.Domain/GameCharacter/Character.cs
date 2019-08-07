using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter
{
    public class Character : Entity
    {
        public Character(Guid id, Guid updateId, DateTime updatedOn, bool isStaff, string playerId, string account, string name, string @class, int gearScore, int level, int playTime, DateTime createdOn, int strength, int dexterity, int stamina, int intelligence, long perin, long penya, long redChips, long euphresiaCoins, long votePoints, long donateCoins, int bossKills, bool isDeleted)
        {
            Id = id;
            UpdateId = updateId;
            UpdatedOn = updatedOn;
            IsStaff = isStaff;
            PlayerId = playerId;
            Account = account;
            Name = name;
            Class = @class;
            GearScore = gearScore;
            Level = level;
            PlayTime = playTime;
            CreatedOn = createdOn;
            Strength = strength;
            Dexterity = dexterity;
            Stamina = stamina;
            Intelligence = intelligence;
            Perin = perin;
            Penya = penya;
            RedChips = redChips;
            EuphresiaCoins = euphresiaCoins;
            VotePoints = votePoints;
            DonateCoins = donateCoins;
            BossKills = bossKills;
            IsDeleted = isDeleted;
        }

        protected Character() { }

        public DateTime UpdatedOn { get;  set; }
        public Guid UpdateId { get;  set; }
        public bool IsStaff { get;  set; }
        public string PlayerId { get;  set; }
        public string Account { get;  set; }
        public string Name { get;  set; }
        public string Class { get;  set; }
        public int GearScore { get;  set; }
        public int Level { get;  set; }
        public int PlayTime { get;  set; }
        public DateTime CreatedOn { get;  set; }
        public int Strength { get;  set; }
        public int Dexterity { get;  set; }
        public int Stamina { get;  set; }
        public int Intelligence { get;  set; }
        public long Perin { get;  set; }
        public long Penya { get;  set; }
        public long RedChips { get;  set; }
        public long EuphresiaCoins { get;  set; }
        public long VotePoints { get;  set; }
        public long DonateCoins { get;  set; }
        public int BossKills { get;  set; }
        public bool IsDeleted { get;  set; }
    }
}
