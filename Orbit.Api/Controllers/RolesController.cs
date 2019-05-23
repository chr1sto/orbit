using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.CrossCutting.Identity.Models;
using Orbit.Infra.CrossCutting.Identity.Models.RoleViewModels;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResult<string[]>), 200)]
        [ProducesResponseType(typeof(ApiResult<string[]>), 400)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return Response<string[]>(null);
            var roles = await _userManager.GetRolesAsync(user);
            return Response(roles.ToArray());
        }

        [HttpPatch("")]
        [ProducesResponseType(typeof(ApiResult<UpdateRolesViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<UpdateRolesViewModel>), 400)]
        [Authorize(Roles = "Administrator,Developer")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateRolesViewModel rolesViewModel)
        {
            var user = await _userManager.FindByIdAsync(rolesViewModel.UserId.ToString());

            if(user == null)
            {
                NotifyError("", "User not found");
                return Response(rolesViewModel);
            }

            /*
            bool rolesExist = true;
            for (int i = 0; i < rolesViewModel.Roles.Length; i++)
            {
                rolesExist = await _roleManager.RoleExistsAsync(rolesViewModel.Roles[i]);
                if (!rolesExist)
                {
                    NotifyError("", "Provided list of roles is invalid!");
                    return Response(rolesViewModel);
                }
            }
            */

            var userRoles = await _userManager.GetRolesAsync(user);

            IdentityResult result;
            if (userRoles != null)
            {
                result = await _userManager.RemoveFromRolesAsync(user, userRoles);
            }          

            result = await _userManager.AddToRolesAsync(user, rolesViewModel.Roles);

            if (!result.Succeeded)
            {
                AddIdentityErrors(result);
            }

            return Response(rolesViewModel);
        }
    }
}
