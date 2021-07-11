using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Interfaces
{
    public interface IStatisticsService
    {
        int GetTotalGold();
        int GetPlayerCount(int channel);
    }
}
