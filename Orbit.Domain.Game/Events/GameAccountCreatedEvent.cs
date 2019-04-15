using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Game.Events
{
    public class GameAccountCreatedEvent : Event
    {
        public GameAccountCreatedEvent(Guid iD, Guid userID, string account, string alias)
        {
            ID = iD;
            UserID = userID;
            Account = account;
            Alias = alias;
        }

        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string Account { get; set; }
        public string Alias { get; set; }
    }
}
