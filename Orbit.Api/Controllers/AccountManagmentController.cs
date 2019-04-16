using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.CrossCutting.Identity.Models;
using Orbit.Infra.CrossCutting.Identity.Extensions;
using Orbit.Infra.CrossCutting.Identity.Services;
using Orbit.Infra.CrossCutting.Identity.Models.ManageViewModels;

namespace Orbit.Api.Controllers
{
    [Route("account-managment")]
    public class AccountManagmentController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IUser _user;
        private readonly IHostingEnvironment _env;
        private readonly string _emailVerificationCallbackUrl;

        public AccountManagmentController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountManagmentController> logger,
            IUser user,
            IHostingEnvironment env,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _user = user;
            _env = env;

            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            _emailVerificationCallbackUrl = config["EMAIL_VERIFICATION_CALLBACK_URL"];
        }

        [HttpPost("resend-verification-mail")]
        public async Task<IActionResult> ResendVerificationMail()
        {
            var user = await GetCurrenUser();

            if(user == null)
            {
                NotifyError("User not found", "Could not resolve current user.");
                return Response();
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var email = user.Email;
            var callback = GetVerificationCallbackUrl(code);
            await _emailSender.SendEmailConfirmationAsync(email, callback);
            return Response("Verification Mail sent.");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(model);
            }

            var user = await GetCurrenUser();

            if (user == null)
            {
                NotifyError("User not found", "Could not resolve current user.");
                return Response(model);
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(!changePasswordResult.Succeeded)
            {
                foreach(var item in changePasswordResult.Errors)
                {
                    NotifyError(item.Code,item.Description);
                }
            }

            return Response(model);
        }

        private string GetVerificationCallbackUrl(string code)
        {
            return _emailVerificationCallbackUrl
                .Replace("{{CODE}}", code)
                .Replace("{{UID}}", _user.Id.ToString());
        }

        private async Task<ApplicationUser> GetCurrenUser()
        {
            if (_user == null || _user.Id == Guid.Empty)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(_user.Id.ToString());

            return user;
        }
    }
}
