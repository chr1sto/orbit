using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IPlayerLogAppService
    {
        void Add(Newtonsoft.Json.Linq.JObject @object);
    }
}
