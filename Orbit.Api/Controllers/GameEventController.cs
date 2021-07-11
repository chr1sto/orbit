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
using Orbit.Domain.Core.Events;
using Orbit.Domain.Core.Notifications;

namespace Orbit.Api.Controllers
{
    [Authorize(Roles = "Developer,GameService,Administrator")]
    [Route("game-events")]
    public class GameEventController : ApiController
    {
        private readonly IGameEventAppService _gameEventAppService;

        public GameEventController(IGameEventAppService gameEventAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _gameEventAppService = gameEventAppService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResult<IList<StoredEvent>>), 200)]
        public IActionResult Get()
        {
            return Response(_gameEventAppService.GetUnhandled());
        }

        [ProducesResponseType(typeof(ApiResult<Guid>), 200)]
        [HttpPatch("")]
        public IActionResult Patch([FromBody] Guid id)
        {
            _gameEventAppService.Handle(id);
            return Response(id);
        }
    }
}
