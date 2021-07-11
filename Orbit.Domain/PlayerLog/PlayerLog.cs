using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.PlayerLog
{
    public class PlayerLog : Entity
    {
        public PlayerLog(Guid id, Guid userId, DateTime timeStamp, string info, string ipAddress)
        {
            Id = id;
            UserId = userId;
            TimeStamp = timeStamp;
            Info = info;
            IpAddress = ipAddress;
        }
        public Guid UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Info { get; set; }
        public string IpAddress { get; set; }
    }
}
