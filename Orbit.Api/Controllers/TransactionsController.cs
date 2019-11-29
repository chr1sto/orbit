using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Authorize]
    [Route("transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ITransactionAppService _transactionAppService;
        private readonly IUser _user;

        public TransactionsController(IUser user,ITransactionAppService transactionAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _transactionAppService = transactionAppService;
            _user = user;
        }

        [ProducesResponseType(typeof(ApiResult<IEnumerable<TransactionViewModel>>), 200)]
        [HttpGet("")]
        [Authorize(Roles = "GameService")]
        public async Task<IActionResult> GetAllPending()
        {
            return Response(_transactionAppService.GetAllPendingForGame());
        }

        [HttpPatch("")]
        [ProducesResponseType(typeof(ApiResult<TransactionViewModel>), 200)]
        [Authorize(Roles = "GameService")]
        public async Task<IActionResult> Update([FromBody] TransactionViewModel viewModel)
        {
            _transactionAppService.Update(viewModel);
            return Response(viewModel);
        }

        [ProducesResponseType(typeof(ApiResult<WithdrawCurrencyViewModel>), 200)]
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawCurrencyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(viewModel);
            }

            var balance = _transactionAppService.GetBalance(_user.Id, viewModel.Currency);

            if(balance < viewModel.Amount)
            {
                NotifyError("NOT_ENOUGH_BALANCE", "The selected amount exceeds your current balance!");
                return Response(viewModel);
            }

            if(viewModel.Currency != "VP" && viewModel.Currency != "DP")
            {
                NotifyError("INVALID_CURRENCY", "You need to select a valid currency!");
                return Response(viewModel);
            }

            _transactionAppService.Add(new Domain.Game.Transaction(Guid.NewGuid(), _user.Id, DateTime.Now, viewModel.Amount * - 1, viewModel.Currency, "localhost", Request.HttpContext.Connection.RemoteIpAddress?.ToString(), "Withdrawal", "GAME", viewModel.Character, "PENDING"));

            return Response(viewModel);
        }
    }
}
