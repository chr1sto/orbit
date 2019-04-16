using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Route("news")]
    public class NewsController : ApiController
    {
        private readonly INewsAppService _newsAppService;

        public NewsController(INewsAppService newsAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _newsAppService = newsAppService;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            //TODO: Async
            //TODO: Send paging info
            var result = _newsAppService.GetAll(true, out int recCount);
            return Response(result);
        }

        [Authorize(Roles = "Administrator,Developer,Gamemaster")]
        [HttpPost("")]
        public IActionResult Post([FromBody]NewsPostViewModel newsViewModel)
        {
            if(!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(newsViewModel);
            }

            _newsAppService.Create(newsViewModel);

            return Response(newsViewModel);
        }

        [Authorize(Roles = "Administrator,Developer,Gamemaster")]
        [HttpPatch("")]
        public IActionResult Patch([FromBody] NewsPostViewModel newsViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(newsViewModel);
            }

            _newsAppService.Update(newsViewModel);

            return Response(newsViewModel);
        }

        [Authorize(Roles = "Administrator,Developer,Gamemaster")]
        [HttpDelete("")]
        public IActionResult Delete(Guid id)
        {
            _newsAppService.Remove(id);

            return Response();
        }
    }
}
