﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.Payments.PayPal.Interfaces;

namespace Orbit.Api.Controllers
{
    [Authorize]
    public class DonateController : ApiController
    {
        private readonly IPayPalService _payPalService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IUser _user;

        public DonateController(IPayPalService payPalService, ITransactionAppService transactionAppService, IUser user, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
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

            var amount = await _payPalService.VerifyOrder(orderId);
            if(amount == 0)
            {
                NotifyError("", "");
                return Response("Something went wrong while verifying your order. Please contact the Euphresia-Staff immediately.");
            }

            _transactionAppService.Add(new Domain.Game.Transaction(Guid.NewGuid(), _user.Id, DateTime.Now, amount, "DP", "localhost", "localhost", $"Donation {orderId}", "WEB", "", "FINISHED"));

            return Response("Successfully Donated!");
        }
    }
}