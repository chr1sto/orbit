using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class GuildWar
    {
        //m_idGuild char(6)
        public string GuildId { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //m_idWar int
        public int WarId { get; set; }
        //f_idGuild char(6)
        public string FGuildId { get; set; }
        //m_nDeath int
        public int DeathCount { get; set; }
        //m_nSurrender int
        public int SurrenderCount { get; set; }
        //m_nAbsent int
        public int AbsentCount { get; set; }
        //f_nDeath int
        public int FDeathCount { get; set; }
        //f_nSurrender int
        public int FSurrenderCount { get; set; }
        //f_nAbsent int
        public int FAbsentCount { get; set; }
        //State char(1)
        public char Sate { get; set; }
        //StartTime char(12)
        public string StartTime { get; set; }
    }
}
