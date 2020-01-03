using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IConcurrencyLockService
    {
        object GetUserLockObject(Guid userId);
    }
}
