using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Commands
{
    public abstract class TransactionCommand : Command
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public DateTime Date { get; protected set; }
        public int Amount { get; protected set; }
        public string Currency { get; protected set; }
        public string IpAddress { get; protected set; }
        public string RemoteAddress { get; protected set; }
        public string Reason { get; protected set; }
    }
}
