using Orbit.Domain.Transaction.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Commands
{
    public class CreateTransactionCommand : TransactionCommand
    {
        public CreateTransactionCommand(Guid userId, DateTime date, int amount, string currency, string ipAddress, string remoteAddress, string reason)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Date = date;
            Amount = amount;
            Currency = currency;
            IpAddress = ipAddress;
            RemoteAddress = remoteAddress;
            Reason = reason;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateTransactionValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
