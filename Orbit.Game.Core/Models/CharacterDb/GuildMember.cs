using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class GuildMember
    {
        //m_idPlayer char(7)
        public string Id { get; set; }
        //serverindex char(2)
        public string ServerIndex { get; set; } = "01";
        //m_idGuild char(6)
        public string GuildId { get; set; }
        //m_szAlias varchar(20)
        public string Alias { get; set; }
        //m_nWin int
        public int WinCount { get; set; }
        //m_nLose int
        public int LooseCount { get; set; }
        //m_nSurrender
        public int SurrenderCount { get; set; }
        //m_nMemberLv
        public int MemberLevel { get; set; }
    }
}
