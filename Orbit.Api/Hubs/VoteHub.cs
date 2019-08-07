using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Hubs
{
    public class VoteHub : Hub
    {
        public Task SendMessageToUser(string user,string code,string message)
        {
            return Clients.User(user).SendAsync(code, message);
        }
    }
}
