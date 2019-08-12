using Orbit.Domain.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Transaction.Validations
{
    public class UpdateTransactionValidation : TransactionValidation<UpdateTransactionCommand>
    {
        public UpdateTransactionValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateReason();
        }
    }
}
