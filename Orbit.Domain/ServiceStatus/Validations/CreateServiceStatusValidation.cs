using Orbit.Domain.ServiceStatus.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.ServiceStatus.Validations
{
    public class CreateServiceStatusValidation : ServiceStatusValidation<CreateServiceStatusCommand>
    {
        public CreateServiceStatusValidation()
        {
            ValidateService();
        }
    }
}
