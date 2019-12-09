using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.Payments.PayPal.Interfaces;

namespace Orbit.Api.Controllers
{
    [Authorize]
    [Route("donate")]
    public class DonateController : ApiController
    {
        private readonly IPayPalService _payPalService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IUser _user;

        public DonateController(IPayPalService payPalService, ITransactionAppService transactionAppService, IUser user, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _payPalService = payPalService;
            _transactionAppService = transactionAppService;
            _user = user;
        }

        [HttpGet("verify-pp-order")]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        public async Task<IActionResult> VerifyPayPalOrder(string orderId)
        {
            if(_transactionAppService.DonateOrderExists(orderId))
            {
                NotifyError("Nice try.", "But keep searching");
                return Response("Something went wrong while verifying your order. Please contact the Euphresia-Staff immediately.");
            }

            var result = await _payPalService.VerifyOrder(orderId);
            if(!result.Success)
            {
                NotifyError("", "");
                _transactionAppService.Add(new Domain.Game.Transaction(Guid.NewGuid(), _user.Id, DateTime.Now, result.Amount, "DP", "localhost", "localhost", $"Donation {orderId}", "WEB", "", "FAILED", result.Info));
                return Response("Something went wrong while verifying your order. Please contact the Euphresia-Staff immediately.");
            }

            _transactionAppService.Add(new Domain.Game.Transaction(Guid.NewGuid(), _user.Id, DateTime.Now, result.Amount, "DP", "localhost", "localhost", $"Donation {orderId}", "WEB", "", "FINISHED",result.Info));

            return Response("Successfully Donated!");
        }

        [ProducesResponseType(typeof(ApiResult<int>), 200)]
        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            return Response(_transactionAppService.GetBalance(_user.Id, "DP"));
        }
    }
}
