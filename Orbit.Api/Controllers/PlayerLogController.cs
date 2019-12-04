using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Route("log-player")]
    public class PlayerLogController : ApiController
    {
        private readonly IPlayerLogAppService _playerLogAppService;
        public PlayerLogController(IPlayerLogAppService playerLogAppService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _playerLogAppService = playerLogAppService;
        }

        [ProducesResponseType(typeof(ApiResult<bool>), 200)]
        [ProducesResponseType(typeof(ApiResult<bool>), 400)]
        [HttpPost("")]
        public IActionResult Post([FromBody] Newtonsoft.Json.Linq.JObject @object)
        {
            _playerLogAppService.Add(@object);
            return Response(true);
        }
    }
}
