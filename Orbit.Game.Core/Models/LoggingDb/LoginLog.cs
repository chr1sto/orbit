using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.LoggingDb
{
    public class LoginLog
    {
        //m_idPlayer (char(7))
        public string PlayerId { get; set; }
        //serverindex (char(2))
        public string ServerIndex { get; set; }
        //dwWorldId (int)
        public int WorldId { get; set; }
        //Start_Time (char(14))
        public DateTime StartTime { get; set; }
        //End_Time (char(14))
        public DateTime EndTime { get; set; }
        //TotalPlayTime (int)
        public int TotalPlayTime { get; set; }
        //m_dwGold (int)
        public int Gold { get; set; }
        //remoteIP (varchar(32))
        public string RemoteIp { get; set; }
        //CharLevel (int)
        public int CharLevel { get; set; }
        //Job (int)
        public int Job { get; set; }
        //State (tinyint)
        public int State { get; set; }
        //SEQ (bigint)
        public long SEQ { get; set; }
    }
}
