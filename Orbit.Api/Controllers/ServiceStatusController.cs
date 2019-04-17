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
    [Route("service-status")]
    public class ServiceStatusController : ApiController
    {
        private readonly IServiceStatusAppService _serviceStatusAppService;


        public ServiceStatusController(IServiceStatusAppService serviceStatusAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _serviceStatusAppService = serviceStatusAppService;
        }

        [ProducesResponseType(typeof(ApiResult<IEnumerable<ServiceStatusViewModel>>), 200)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer,GameService")]
        [HttpGet("recent-hidden")]
        public IActionResult GetRecent()
        {
            return Response(_serviceStatusAppService.GetRecent());
        }

        [ProducesResponseType(typeof(ApiResult<ServiceStatusViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<ServiceStatusViewModel>), 400)]
        [Authorize(Roles = "GameService")]
        [HttpPost]
        public IActionResult Post([FromBody]ServiceStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(model);
            }

            _serviceStatusAppService.Create(model);
            return Response(model);
        }

        [ProducesResponseType(typeof(ApiResult<ServiceStatusViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<ServiceStatusViewModel>), 400)]
        [Authorize(Roles = "GameService")]
        [HttpPatch]
        public IActionResult Patch([FromBody]ServiceStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(model);
            }

            _serviceStatusAppService.Update(model);
            return Response(model);
        }

        [ProducesResponseType(typeof(Guid), 200)]
        [Authorize(Roles = "GameService")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            _serviceStatusAppService.Remove(id);
            return Response(id);
        }
    }
}
