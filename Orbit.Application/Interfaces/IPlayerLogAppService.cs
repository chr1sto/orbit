using Orbit.Domain.PlayerLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IPlayerLogAppService
    {
        void Add(Newtonsoft.Json.Linq.JObject @object);
        IEnumerable<PlayerLog> GetByUser( Guid userId, out int recordCount, int pageIndex = 0, int recordsPerPage = 10);
    }
}
