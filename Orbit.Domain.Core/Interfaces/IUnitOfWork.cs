using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Core.Interfaces
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}
