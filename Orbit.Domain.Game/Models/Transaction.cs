using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Game
{ 
    public class Transaction : Entity
    {
        public Transaction(Guid id, Guid userId, DateTime date, int amount, string currency, string ipAddress, string remoteAddress, string reason, string target, string targetInfo, string status, string additionalInfo)
        {
            Id = id;
            UserId = userId;
            Date = date;
            Amount = amount;
            Currency = currency;
            IpAddress = ipAddress;
            RemoteAddress = remoteAddress;
            Reason = reason;
            Target = target;
            TargetInfo = targetInfo;
            Status = status;
            AdditionalInfo = additionalInfo;
        }

        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string IpAddress { get; set; } 
        public string RemoteAddress { get; set; }
        public string Reason { get; set; }
        public string Target { get; set; }
        public string TargetInfo { get; set; }
        public string Status { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
