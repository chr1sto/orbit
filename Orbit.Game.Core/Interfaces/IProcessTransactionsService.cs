using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Interfaces
{
    public interface IProcessTransactionsService
    {
        Task Process();
    }
}
