using Orbit.Domain.Generic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Validations
{
    public class RemoveGenericObjectValidation : GenericObjectValidation<RemoveGenericObjectCommand>
    {
        public RemoveGenericObjectValidation()
        {
            ValidateId();
        }
    }
}
