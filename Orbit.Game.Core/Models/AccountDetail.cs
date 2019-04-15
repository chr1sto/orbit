using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Models
{
    public class AccountDetail
    {
        public string AccountId { get; set; }
        public string GameCode { get; set; }
        public char Tester { get; set; }
        public char LoginAuthority { get; set; }
        public DateTime RegDate { get; set; }
        public string BlockTime { get; set; }
        public string EndTime { get; set; }
        public string WebTime { get; set; }
        public char IsUse { get; set; }
        public DateTime? Secession { get; set; }
        public string Email { get; set; }
    }
}
