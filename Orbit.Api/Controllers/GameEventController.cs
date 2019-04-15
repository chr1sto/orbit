using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Events;
using Orbit.Domain.Core.Notifications;

namespace Orbit.Api.Controllers
{
    [Route("game-events")]
    public class GameEventController : ApiController
    {
        private readonly IGameEventAppService _gameEventAppService;

        public GameEventController(IGameEventAppService gameEventAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _gameEventAppService = gameEventAppService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public IActionResult Get()
        {
            return Response(_gameEventAppService.GetUnhandled());
        }

        [HttpPatch("")]
        public IActionResult Patch([FromBody] Guid id)
        {
            _gameEventAppService.Handle(id);
            return Response(id);
        }
    }
}
