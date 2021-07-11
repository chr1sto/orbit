using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ViewModels
{
    public class CharacterViewModel
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int GearScore { get; set; }
        public int Level { get; set; }
        public int PlayTime { get; set; }   
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }        
        public int BossKills { get; set; }
    }
}
