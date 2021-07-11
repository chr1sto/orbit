using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Hubs
{
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class VoteHub : Hub
    {
        public VoteHub()
        {

        }

        public Task SendMessageToUser(string user,string code,string message)
        {
            return Clients.User(user).SendAsync(code, message);
        }
    }
}
