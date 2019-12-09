using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.Payments.PayPal.Models
{
    public class PayPalResult
    {
        public PayPalResult(string response)
        {
            Response = response;
        }

        public string Response { get; set; }
    }
}
