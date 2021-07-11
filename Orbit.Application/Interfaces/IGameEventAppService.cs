using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IGameEventAppService
    {
        void Handle(Guid id);
        IList<StoredEvent> GetUnhandled();
    }
}
