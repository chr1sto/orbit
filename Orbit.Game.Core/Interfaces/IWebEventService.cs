using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Interfaces
{
    public interface IWebEventService
    {
        Task<ICollection<StoredEvent>> GetUnhandled();
        Task<bool> SetHandled(Guid id);
    }
}
