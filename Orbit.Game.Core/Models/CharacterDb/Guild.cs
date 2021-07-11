using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class Guild
    {
        //m_idGuild char(6)
        public string Id { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //m_szGuild varchar(16)
        public string Name { get; set; }
        //m_nLevel int
        public int Level { get; set; }
        // m_nWin int
        public int WinCount { get; set; }
        // m_nLose int
        public int LooseCount { get; set; }
        // m_nSurrencer int
        public int SurrenderCount { get; set; }

    }
}
