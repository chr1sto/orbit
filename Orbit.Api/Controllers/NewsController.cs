using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IPagedList<NewsPostViewModel>>>), 200)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer")]
        [HttpGet("unpublished")]
        public IActionResult GetAll([FromQuery] int index = 0,[FromQuery] int count = 10)
        {
            var result = _newsAppService.GetAll(false, out int recCount, index, count);
            var pagedResultData = new PagedResultData<IPagedList<NewsPostViewModel>>(result, recCount, index, count);
            return Response(pagedResultData);
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IPagedList<NewsPostViewModel>>>), 200)]
        [HttpGet("")]
        public IActionResult Get([FromQuery] int index = 0, [FromQuery] int count = 10)
        {
            //TODO: Async
            //TODO: Send paging info
            var result = _newsAppService.GetAll(true, out int recCount);
            var pagedResultData = new PagedResultData<IPagedList<NewsPostViewModel>>(result, recCount, index, count);
            return Response(pagedResultData);
        }

        [ProducesResponseType(typeof(ApiResult<NewsPostViewModel>), 200)]
        [HttpGet(template: "{id}",Name = "ById_Get_News")]
        public IActionResult Get(Guid id)
        {
            var result = _newsAppService.GetSingle(id);
            return Response(result);
        }

        [ProducesResponseType(typeof(ApiResult<NewsPostViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<NewsPostViewModel>), 400)]
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

        [ProducesResponseType(typeof(ApiResult<NewsPostViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<NewsPostViewModel>), 400)]
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

        [ProducesResponseType(typeof(ApiResult<Guid>), 200)]
        [ProducesResponseType(typeof(ApiResult<Guid>), 400)]
        [Authorize(Roles = "Administrator,Developer,Gamemaster")]
        [HttpDelete("")]
        public IActionResult Delete(Guid id)
        {
            _newsAppService.Remove(id);

            return Response(id);
        }
    }
}
