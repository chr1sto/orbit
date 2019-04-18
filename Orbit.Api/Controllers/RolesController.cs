using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.CrossCutting.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    [Authorize(Roles = "Administrator,Developer,Gamemaster")]
    [Route("roles")]
    public class RolesController : ApiController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(UserManager<ApplicationUser> userManager,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userManager = userManager;
        }

        [HttpGet("{userid}")]
        [ProducesResponseType(typeof(ApiResult<string[]>), 200)]
        [ProducesResponseType(typeof(ApiResult<string[]>), 400)]
        public async Task<IActionResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Response<string[]>(null);
            var roles = await _userManager.GetRolesAsync(user);
            return Response(roles.ToArray());
        }
    }
}
