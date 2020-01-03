using Orbit.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Services
{
    public class ConcurrencyLockService : IConcurrencyLockService
    {
        private readonly Dictionary<Guid, object> _locks;

        public ConcurrencyLockService()
        {
            _locks = new Dictionary<Guid, object>();
        }

        public object GetUserLockObject(Guid userId)
        {
            return _locks[userId];
        }
    }
}
