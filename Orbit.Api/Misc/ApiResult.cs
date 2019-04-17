using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Misc
{
    public class ApiResult<T>
    {
        public ApiResult(in T data, bool success, IEnumerable<string> errors = null)
        {
            Data = data;
            Success = success;
            Errors = errors;
        }

        public T Data { get; private set; }
        public bool Success { get; private set; }
        public IEnumerable<string> Errors { get; private set; }
    }
}
