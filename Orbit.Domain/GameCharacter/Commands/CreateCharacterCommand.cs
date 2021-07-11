using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter.Commands
{
    public class CreateCharacterCommand : CharacterCommand
    {
        public CreateCharacterCommand(DateTime updatedOn, Guid updateId, bool isStaff, string playerId, string account, string name, string @class, int gearScore, int level, int playTime, DateTime createdOn, int strength, int dexterity, int stamina, int intelligence, long perin, long penya, long redChips, long euphresiaCoins, long votePoints, long donateCoins, int bossKills, bool isDeleted)
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
            Penya = penya;
            RedChips = redChips;
            EuphresiaCoins = euphresiaCoins;
            VotePoints = votePoints;
            DonateCoins = donateCoins;
            BossKills = bossKills;
            IsDeleted = isDeleted;
        }

        public override bool IsValid()
        {
            //TODO Do THIS later
            return true;
        }
    }
}
