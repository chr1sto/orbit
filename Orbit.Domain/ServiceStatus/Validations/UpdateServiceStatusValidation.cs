using Orbit.Domain.ServiceStatus.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Validations
{
    public class UpdateServiceStatusValidation : ServiceStatusValidation<UpdateServiceStatusCommand>
    {
        public UpdateServiceStatusValidation()
        {
            ValidateService();
            ValidateId();
        }
    }
}
