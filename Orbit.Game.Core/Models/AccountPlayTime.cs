using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models
{
    public class AccountPlayTime
    {
        public string AccountId { get; set; }
        public int PlayDate { get; set; }
        public int PlayTime { get; set; }
    }
}
