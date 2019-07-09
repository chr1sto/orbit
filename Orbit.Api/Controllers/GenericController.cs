using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;

namespace Orbit.Api.Controllers
{
    [Route("general")]
    public class GenericController : ApiController
    {
        private readonly IGenericObjectAppService _genericObjectAppService;

        public GenericController(IGenericObjectAppService genericObjectAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _genericObjectAppService = genericObjectAppService;
        }

        [ProducesResponseType(typeof(ApiResult<IEnumerable<GenericObjectViewModel>>), 200)]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GenericObjectViewModel>>), 400)]
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] string @type,[FromQuery] int amount)
        {
            var result = _genericObjectAppService.GetAll(@type, amount);
            return Response(result);
        }

        [ProducesResponseType(typeof(ApiResult<GenericObjectViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<GenericObjectViewModel>), 400)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer,GameService")]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] GenericObjectViewModel model)
        {
            _genericObjectAppService.Create(model);
            return Response(model);
        }

        [ProducesResponseType(typeof(ApiResult<GenericObjectViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<GenericObjectViewModel>), 400)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer,GameService")]
        [HttpPatch("")]
        public async Task<IActionResult> Update([FromBody] GenericObjectViewModel model)
        {
            _genericObjectAppService.Update(model);
            return Response(model);
        }

        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer,GameService")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _genericObjectAppService.Remove(new Guid(id));
            return Response(id);
        }
    }
}
