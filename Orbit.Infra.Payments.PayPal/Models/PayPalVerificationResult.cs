using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.Payments.PayPal.Models
{
    public class PayPalVerificationResult
    {
        public PayPalVerificationResult(int amount, bool success, string info)
        {
            Amount = amount;
            Success = success;
            Info = info;
        }

        public int Amount { get; set; }
        public bool Success { get; private set; }
        public string Info { get; set; }
    }
}
