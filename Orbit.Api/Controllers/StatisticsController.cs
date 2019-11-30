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

namespace Orbit.Api.Controllers
{
    [Route("statistics")]
    [Authorize(Roles = "Administrator,Developer,Gamemaster,GameService")]
    public class StatisticsController : ApiController
    {
        private readonly IUser _user;
        private readonly IStatisticsAppService _statisticsAppService;
        public StatisticsController(IUser user,IStatisticsAppService statisticsAppService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _user = user;
            _statisticsAppService = statisticsAppService;
        }

        [ProducesResponseType(typeof(ApiResult<IEnumerable<StatisticsEntryViewModel>>), 200)]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<StatisticsEntryViewModel>>), 400)]
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] string from, [FromQuery] string until, [FromQuery] string statGroup, [FromQuery] string statName = null, [FromQuery] char interval = 'h')
        {
            var result = _statisticsAppService.Get(DateTime.Parse(from), DateTime.Parse(until), statGroup, statName, interval);

            return Response(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] StatisticsEntryViewModel model)
        {
            _statisticsAppService.Create(model);

            return Response(model);
        }
    }
}
