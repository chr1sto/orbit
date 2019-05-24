using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter
{
    public class Character : Entity
    {
        public Character(Guid id, Guid updateId, DateTime updatedOn, bool isStaff, string playerId, string account, string name, string @class, int gearScore, int level, int playTime, DateTime createdOn, int strength, int dexterity, int stamina, int intelligence, int perin, int redChips, int euphresiaCoins, int votePoints, int donateCoins, int bossKills)
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
            RedChips = redChips;
            EuphresiaCoins = euphresiaCoins;
            VotePoints = votePoints;
            DonateCoins = donateCoins;
            BossKills = bossKills;
        }

        protected Character() { }

        public DateTime UpdatedOn { get; private set; }
        public Guid UpdateId { get; private set; }
        public bool IsStaff { get; private set; }
        public string PlayerId { get; private set; }
        public string Account { get; private set; }
        public string Name { get; private set; }
        public string Class { get; private set; }
        public int GearScore { get; private set; }
        public int Level { get; private set; }
        public int PlayTime { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Stamina { get; private set; }
        public int Intelligence { get; private set; }
        public int Perin { get; private set; }
        public int RedChips { get; private set; }
        public int EuphresiaCoins { get; private set; }
        public int VotePoints { get; private set; }
        public int DonateCoins { get; private set; }
        public int BossKills { get; private set; }
    }
}
