using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class CombatJoinPlayer
    {
        //CombatID int
        public int CombatId { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //GuildID char(6)
        public string GuildId { get; set; }
        //PlayerID char(7) { get; set; }
        public string PlayerId { get; set; }
        //Status varchar(3)
        public string Status { get; set; }
        //Point int
        public int Point { get; set; }
        //Reward bigint
        public BigInteger Reward { get; set; }
        //RewardDt datetime
        public DateTime RewardDate { get; set; }
    }
}
