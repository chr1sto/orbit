using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class Character
    {
        // m_idPlayer char (7)
        public string IdPlayer { get; set; }
        // account varchar(32)
        public string Account { get; set; }
        // m_szName varchar(32)
        public string Name { get; set; }
        // m_dwGold int
        public int Gold { get; set; } 
        // m_nJob int
        public int Job { get; set; }
        // m_nStr int
        public int Str { get; set; }
        // m_nSta int
        public int Sta { get; set; }
        // m_nDex int
        public int Dex { get; set; }
        // m_nInt int
        public int Int { get; set; }
        // m_nLevel int
        public int Level { get; set; }
        // m_nAuthority char(1)
        public char Authority { get; set; }
        // TotalPlayTime int
        public int TotalPlayTime { get; set; }
        // m_nPerin bigint
        public long Perin { get; set; }
        // m_nFarm bigint
        public long Farm { get; set; }
        //m_nDonate bigint
        public long Donate { get; set; }
        //m_nVote bigint
        public long Vote { get; set; }
        //m_nChips bigint
        public long Chips { get; set; }
        //m_nBossKilss int
        public int? BossKills { get; set; }
        //m_nGearScore int
        public int GearScore { get; set; }
        //isblock char(1)
        public char IsBlock { get; set; }
        //int
        public int MultiServer { get; set; }

    }
}
