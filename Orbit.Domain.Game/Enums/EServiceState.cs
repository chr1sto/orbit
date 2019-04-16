using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Game.Enums
{
    public enum EServiceState
    {
        Offline = 0,
        Starting = 1,
        Restarting = 2,
        Online = 3,
        Maintenance = 4
    }
}
