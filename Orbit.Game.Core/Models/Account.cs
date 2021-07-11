using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models
{
    public class Account
    {
        public string AccountId { get; set; }
        public string Password { get; set; }
        public char IsUse { get; set; }
        public char Member { get; set; }
        public string IdNo1 { get; set; }
        public string IdNo2 { get; set; }
        public char RealName { get; set; }
        public char Reload { get; set; }
        public string OldPassword { get; set; }
        public string TempPassword { get; set; }
        public int Cash { get; set; }
        public string UserId { get; set; }
    }
}
