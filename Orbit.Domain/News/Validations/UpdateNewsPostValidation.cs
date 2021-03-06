using Orbit.Domain.News.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Validations
{
    public class UpdateNewsPostValidation : NewsPostValidation<UpdateNewsPostCommand>
    {
        public UpdateNewsPostValidation()
        {
            ValidateCaption();
            ValidateId();
        }
    }
}
