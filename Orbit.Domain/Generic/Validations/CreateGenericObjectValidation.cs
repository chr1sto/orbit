using Orbit.Domain.Generic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Validations
{
    public class CreateGenericObjectValidation : GenericObjectValidation<CreateGenericObjectCommand>
    {
        public CreateGenericObjectValidation()
        {
            ValidateType();
        }
    }
}
