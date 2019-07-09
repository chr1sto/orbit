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
        public long Perin { get; set; }
        public long Penya { get; set; }
        public long RedChips { get; set; }
        public long EuphresiaCoins { get; set; }
        public long VotePoints { get; set; }
        public long DonateCoins { get; set; }
        public int BossKills { get; set; }
        public bool IsDeleted { get; set; }
    }
}
