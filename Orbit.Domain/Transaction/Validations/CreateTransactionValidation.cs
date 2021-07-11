using Orbit.Domain.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Validations
{
    public class CreateTransactionValidation : TransactionValidation<CreateTransactionCommand>
    {
        public CreateTransactionValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateReason();
        }
    }
}
