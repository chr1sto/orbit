using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class CharacterAdminViewModel
    {
        public Guid Id { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdateId { get; set; }
        public bool IsStaff { get; set; }
        public string PlayerId { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int GearScore { get; set; }
        public int Level { get; set; }
        public int PlayTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }
        public int Perin { get; set; }
        public int RedChips { get; set; }
        public int EuphresiaCoins { get; set; }
        public int VotePoints { get; set; }
        public int DonateCoins { get; set; }
        public int BossKills { get; set; }
    }
}
