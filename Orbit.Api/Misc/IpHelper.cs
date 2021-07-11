using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Misc
{
    public static class IPhelper
    {
        public static string GetIPAddress(this HttpRequest Request)
        {
            if (Request.Headers.ContainsKey("CF-CONNECTING-IP") && Request.Headers["CF-CONNECTING-IP"].FirstOrDefault() != null)
                return Request.Headers["CF-CONNECTING-IP"].ToString();

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"].ToString();


            return Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
