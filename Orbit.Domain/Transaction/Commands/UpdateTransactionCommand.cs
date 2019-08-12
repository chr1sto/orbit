using Orbit.Domain.Transaction.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Commands
{
    public class UpdateTransactionCommand : TransactionCommand
    {
        public UpdateTransactionCommand(Guid id, Guid userId, DateTime date, int amount, string currency, string ipAddress, string remoteAddress, string reason, string target, string targetInfo, string status)
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
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateTransactionValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
