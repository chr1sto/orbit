using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter.Commands
{
    public class CreateCharacterCommand : CharacterCommand
    {
        public CreateCharacterCommand(DateTime updatedOn, Guid updateId, bool isStaff, string playerId, string account, string name, string @class, int gearScore, int level, int playTime, DateTime createdOn, int strength, int dexterity, int stamina, int intelligence, int perin, int redChips, int euphresiaCoins, int votePoints, int donateCoins, int bossKills)
        {
            Id = Guid.NewGuid();
            UpdatedOn = updatedOn;
            UpdateId = updateId;
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

        public override bool IsValid()
        {
            //TODO Do THIS later
            return true;
        }
    }
}
