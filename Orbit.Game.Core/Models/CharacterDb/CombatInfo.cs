using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class CombatInfo
    {
        //CombatID int
        public int CombatId { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //status varchar(3)
        public string Status { get; set; }
        //StartDate DateTime
        public DateTime? StartDate { get; set; }
        //EndDate DateTime
        public DateTime? EndDate { get; set; }
        //Comment varchar(1000)
        public string Comment { get; set; }
        //SEQ bigint
        public BigInteger Seq { get; set; }
    }
}
