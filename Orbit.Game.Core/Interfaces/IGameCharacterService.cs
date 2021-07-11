using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Interfaces
{
    public interface IGameCharacterService
    {
        IEnumerable<Character> GetAll();
        void UpdateAll();
    }
}
