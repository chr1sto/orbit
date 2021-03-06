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
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Game.Models;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using X.PagedList;

namespace Orbit.Api.Controllers
{
    [Route("game-account")]
    [Authorize]
    public class GameAccountController : ApiController
    {
        private readonly IGameAccountAppService _gameAccountAppService;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public GameAccountController(IUser user,IGameAccountAppService gameAccountAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _gameAccountAppService = gameAccountAppService;
            _user = user;
        }

        [ProducesResponseType(typeof(ApiResult<GameAccountViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<GameAccountViewModel>), 400)]
        [HttpPost("")]
        public IActionResult Create([FromBody] GameAccountViewModel gameAccountViewModel)
        {
            if(!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(gameAccountViewModel);
            }

            _gameAccountAppService.Create(gameAccountViewModel);

            return Response(gameAccountViewModel);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Developer,Gamemaster")]
        [ProducesResponseType(typeof(ApiResult<IPagedList<GameAccountViewModel>>), 200)]
        public IActionResult GetUserGameAccounts(Guid id, string server = "LIVE")
        {
            var gameAccounts = _gameAccountAppService.GetAll(id, true, out int recCount,server: server);
            return Response(gameAccounts);
        }

        [ProducesResponseType(typeof(ApiResult<GameAccountViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<GameAccountViewModel>), 400)]
        [HttpPatch("")]
        public IActionResult Update([FromBody] GameAccountViewModel gameAccountViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(gameAccountViewModel);
            }

            _gameAccountAppService.Update(gameAccountViewModel);

            return Response(gameAccountViewModel);
        }

        [ProducesResponseType(typeof(ApiResult<IPagedList<GameAccountViewModel>>), 200)]
        //TODO QUERY PARAMS FOR PAGES
        [HttpGet("")]
        public IActionResult Get(string server = "LIVE")
        {
            var gameAccounts = _gameAccountAppService.GetAll(_user.Id, true, out int reccCount,server: server);
            return Response(gameAccounts);
        }
    }
}
