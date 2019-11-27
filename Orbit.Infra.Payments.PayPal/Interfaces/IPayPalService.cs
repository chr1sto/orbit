using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Infra.Payments.PayPal.Interfaces
{
    public interface IPayPalService
    {
        Task<int> VerifyOrder(string orderId);
    }
}
