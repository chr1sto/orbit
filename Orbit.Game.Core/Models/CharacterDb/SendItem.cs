using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models.CharacterDb
{
    public class SendItem
    {
        public int Key { get; set; }
        public string PlayerId { get; set; }
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
        public string SenderId { get; set; }
        public string ServerIndex { get; set; }

        public int AbilityObtion { get; set; } = 0;
        public int ItemResist { get; set; } = 0;
        public int ResistAbilityOption { get; set; } = 0;
        public int Charged { get; set; } = 0;
        public int PiercedSize { get; set; } = 0;
        public int DwItemId0 { get; set; } = 0;
        public int DwItemId1 { get; set; } = 0;
        public int DwItemId2 { get; set; } = 0;
        public int DwItemId3 { get; set; } = 0;
        public long KeepTime { get; set; } = 0;
        public int ItemFlag { get; set; } = 0;
        public DateTime ReceiveDate { get; set; } = DateTime.MaxValue;

    }
}
