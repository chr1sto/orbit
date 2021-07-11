using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Game.Models
{
    public class GameAccount : Entity
    {
        public GameAccount(Guid id, Guid userID, string account, string alias, string server)
        {
            Id = id;
            UserID = userID;
            Account = account;
            Alias = alias;
            Server = server;
        }

        public Guid UserID { get; set; }
        public string Account { get; set; }
        public string Alias { get; set; }
        public string Server { get; set; }
    }
}
