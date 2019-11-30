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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Route("game-character")]
    [Authorize]
    public class GameCharacterController : ApiController
    {
        private readonly IGameCharacterAppService _gameCharacterAppService;
        private readonly IGameAccountAppService _gameAccountAppService;
        private readonly IUser _user;

        public GameCharacterController(IUser user, IGameAccountAppService gameAccountAppService,IGameCharacterAppService gameCharacterAppService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMemoryCache cache) : base(notifications, mediator, cache)
        {
            _gameCharacterAppService = gameCharacterAppService;
            _gameAccountAppService = gameAccountAppService;
            _user = user;
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterAdminViewModel>>>), 200)]
        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<CharacterAdminViewModel>>>), 400)]
        [Authorize(Roles = "Administrator,Developer,Gamemaster,GameService")]
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
                var result = _gameCharacterAppService.GetAllByGameAccount(id,true,out int total, index, count, searchText);
                var pagedApiResult = new PagedResultData<IEnumerable<CharacterAdminViewModel>>(result, total, index, count);
                return Response(pagedApiResult);
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "Administrator,Developer,Gamemaster,GameService")]
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

        [HttpGet("my-characters")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<CharacterViewModel>>), 200)]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<CharacterViewModel>>), 400)]
        public async Task<IActionResult> GetUserCharacters()
        {
            List<CharacterViewModel> characters = new List<CharacterViewModel>();
            var gameAccounts = _gameAccountAppService.GetAll(_user.Id, true, out int recCount, 0, 100);
            foreach(var item in gameAccounts)
            {
                var chars = _gameCharacterAppService.GetAllByGameAccount(item.Account, false, out int total);
                characters.AddRange(chars.Select(x => new CharacterViewModel() {
                    BossKills = x.BossKills,
                    Class = x.Class,
                    Dexterity = x.Dexterity,
                    GearScore = x.GearScore,
                    Intelligence = x.Intelligence,
                    Level = x.Level,
                    Name = x.Name,
                    PlayTime = x.PlayTime,
                    Stamina = x.Stamina,
                    Strength = x.Strength
                }));
            }

            return Response(characters);
        }
    }
}
