using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Route("ranking")]
    [AllowAnonymous]
    public class RankingController : ApiController
    {
        private readonly IGameCharacterAppService _characterService;
        public RankingController(IGameCharacterAppService characterService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _characterService = characterService;
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterViewModel>>>), 200)]
        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterViewModel>>>), 400)]
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] string orderBy = "", [FromQuery] string job = "")
        {
            var result = _characterService.GetRanking(out int total, index, count, orderBy, job);
            var pagedApiResult = new PagedResultData<IEnumerable<CharacterViewModel>>(result, total, index, count);
            return Response(pagedApiResult);
        }
    }
}
