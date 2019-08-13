﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Orbit.Api.Hubs;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;

namespace Orbit.Api.Controllers
{
    [AllowAnonymous]
    [Route("vote")]
    public class VoteController : ApiController
    {
        private const int VOTE_POINTS = 8;
        private readonly string[] validAddresses = { "104.28.15.89", "173.245.58.198","173.245.59.206","127.0.0.1"};
        private readonly ITransactionAppService _transactionAppService;
        private readonly IGameCharacterAppService _gameCharacterAppService;
        private readonly IHubContext<VoteHub> _voteHubContext;
        private readonly IUser _user;

        public VoteController(IUser user,ITransactionAppService transactionAppService, IGameCharacterAppService gameCharacterAppService,IHubContext<VoteHub> voteHubContext,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _transactionAppService = transactionAppService;
            _voteHubContext = voteHubContext;
            _gameCharacterAppService = gameCharacterAppService;
            _user = user;
        }

        [HttpPost("pingback")]
        public async Task<IActionResult> Pingback(string VoterIP, string Successful, string Reason, string pingUsername)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

#if DEBUG
#else
            if(!validAddresses.Contains(remoteIpAddress.ToString()))
            {
                return BadRequest("NICE_TRY");
            }
#endif
            bool transferToChar = false;
            string charName = null;
            string userID = null;
            if (!Guid.TryParse(pingUsername,out Guid result))
            {
                charName = pingUsername;
                transferToChar = true;
                var userId = _gameCharacterAppService.GetWebIdFromCharName(charName);
                if (userId == Guid.Empty) return BadRequest();
                userID = userId.ToString();
            }
            else
            {
                userID = pingUsername;
            }

            //TODO Get ID and set flag to automatically send VotePoints to character!

            var lastVote = _transactionAppService.GetLastVote(new Guid(userID), VoterIP);

            bool canVote = true;
            TimeSpan elapsedTime = new TimeSpan(0,0,0);
            if (lastVote != null)
            {
                elapsedTime = DateTime.Now - lastVote.Date;
                if (elapsedTime < new TimeSpan(24, 0, 0))
                {
                    canVote = false;
                }
            }

            if(!canVote)
            {
                var timeUntilNextVote = new TimeSpan(24, 0, 0) - elapsedTime;
                if(!transferToChar) await _voteHubContext.Clients.User(userID).SendAsync("STATE", new VoteState("ALREADY_VOTED",timeUntilNextVote.ToString()));
                return Ok();
            }

            if (Successful == "1")
            {
                _transactionAppService.Add(new Orbit.Domain.Game.Transaction(Guid.NewGuid(), new Guid(userID), DateTime.Now, VOTE_POINTS, "VP", VoterIP, remoteIpAddress.ToString(), "GTOP 100","WEB","","FINISHED"));

                if(transferToChar)
                {
                    _transactionAppService.Add(new Domain.Game.Transaction(Guid.NewGuid(), new Guid(userID), DateTime.Now, -8, "VP", VoterIP, remoteIpAddress.ToString(), "Withdrawal", "GAME", charName, "PENDING"));
                }

                if (!transferToChar)  await _voteHubContext.Clients.User(userID).SendAsync("STATE", new VoteState("VOTE_SUCCESFULL", ""));
            }
            else
            {
                if (!transferToChar)  await _voteHubContext.Clients.User(userID).SendAsync("STATE",new VoteState("VOTE_FAILED",""));
            }


            return Ok();
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<TransactionViewModel>>>), 200)]
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetVoteTransactionHistory([FromQuery] int index = 0, [FromQuery] int count = 20)
        {
            var id = _user.Id;
            var transactions = _transactionAppService.GetAllByUser(id, out int recordCount, index, count);
            var pagedResultData = new PagedResultData<IEnumerable<TransactionViewModel>>(transactions, recordCount, index, count);
            return Response(pagedResultData);
        }

        [ProducesResponseType(typeof(ApiResult<int>), 200)]
        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            return Response(_transactionAppService.GetBalance(_user.Id, "VP"));
        }

        [ProducesResponseType(typeof(ApiResult<VoteState>), 200)]
        [Authorize]
        [HttpGet("status")]
        public async Task<IActionResult> CanVote()
        {
            var lastVote = _transactionAppService.GetLastVote(_user.Id, Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            if(lastVote != null)
            {
                var elapsedTime = DateTime.Now - lastVote.Date;
                if (elapsedTime < new TimeSpan(24, 0, 0))
                {
                    var timeUntilNextVote = new TimeSpan(24, 0, 0) - elapsedTime;
                    return Response(new VoteState("ALREADY_VOTED", timeUntilNextVote.ToString()));
                }
            }
            return Response(new VoteState("CAN_VOTE", ""));
        }
    }
}
