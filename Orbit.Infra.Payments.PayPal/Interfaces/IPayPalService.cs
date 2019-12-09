using Orbit.Infra.Payments.PayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Infra.Payments.PayPal.Interfaces
{
    public interface IPayPalService
    {
        Task<PayPalVerificationResult> VerifyOrder(string orderId);
    }
}
