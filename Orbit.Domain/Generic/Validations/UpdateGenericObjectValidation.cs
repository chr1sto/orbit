using Orbit.Domain.Generic.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic.Validations
{
    public class UpdateGenericObjectValidation : GenericObjectValidation<UpdateGenericObjectCommand>
    {
        public UpdateGenericObjectValidation()
        {
            ValidateId();
            ValidateType();
        }
    }
}
