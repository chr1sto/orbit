using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class CombatJoinGuild
    {
        //CombatID int
        public int CombatId { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //GuildID char(6)
        public string GuildId { get; set; }
        //Status varchar(3)
        public string Status { get; set; }
        //ApplyDt DateTime
        public DateTime ApplyDate { get; set; }
        //CombatFee bigint
        public BigInteger CombatFee { get; set; }
        //ReturnCombatFee bigint
        public BigInteger ReturnCombatFee { get; set; }
        //Reward bigint
        public BigInteger Reward { get; set; }
        //Point int
        public int Point { get; set; }
        //RewardDt DateTime
        public DateTime RewardDate { get; set; }
        //CancelDt DateTime
        public DateTime CancelDate { get; set; }
        //SEQ bigint
        public BigInteger Seq { get; set; }
    }
}
