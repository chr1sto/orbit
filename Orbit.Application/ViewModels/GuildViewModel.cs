using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class GuildViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CharacterViewModel Leader { get; set; }
        public int Level { get; set; }
        public int AverageGearScore { get; set; }
        public int TotalGearScore { get; set; }
        public int MemberCount { get; set; }
        public int Score { get; set; }
    }
}
