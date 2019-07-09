using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Route("game-character")]
    [Authorize(Roles = "Administrator,Developer,Gamemaster,GameService")]
    public class GameCharacterController : ApiController
    {
        private readonly IGameCharacterAppService _gameCharacterAppService;
        public GameCharacterController(IGameCharacterAppService gameCharacterAppService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _gameCharacterAppService = gameCharacterAppService;
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterAdminViewModel>>>), 200)]
        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterAdminViewModel>>>), 400)]
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string id = null, [FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] string searchText = "")
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                var result = _gameCharacterAppService.GetCurrent(out int total, index, count, searchText);
                var pagedApiResult = new PagedResultData<IEnumerable<CharacterAdminViewModel>>(result, total, index, count);
                return Response(pagedApiResult);
            }
            else
            {
                var result = _gameCharacterAppService.GetAllByGameAccount(id,out int total, index, count, searchText);
                var pagedApiResult = new PagedResultData<IEnumerable<CharacterAdminViewModel>>(result, total, index, count);
                return Response(pagedApiResult);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CharacterAdminViewModel> models)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(models);
            }

            _gameCharacterAppService.InsertNewEntries(models);
            return Response(models);
        }
    }
}
