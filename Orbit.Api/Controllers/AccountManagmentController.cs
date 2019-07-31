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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Orbit.Api.Misc;
using Orbit.Infra.CrossCutting.Identity.Models.AccountViewModels;
using System.Text.Encodings.Web;

namespace Orbit.Api.Controllers
{
    [Authorize]
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
        private readonly string _passwordResetCallbackUrl;

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
            _passwordResetCallbackUrl = config["PASSWORD_RESET_CALLBACK_URL"];
        }

        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [HttpPost("resend-verification-mail")]
        public async Task<IActionResult> ResendVerificationMail()
        {
            var user = await GetCurrenUser();

            if(user == null)
            {
                NotifyError("User not found", "Could not resolve current user.");
                return Response<object>(null);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var email = user.Email;
            var callback = GetVerificationCallbackUrl(code);
            await _emailSender.SendEmailConfirmationAsync(email, callback);
            return Response("Verification Mail sent.");
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [HttpPost("verify-mail")]
        public async Task<IActionResult> VerifyMail([FromQuery] string code, [FromQuery] string id)
        {
            var user =  await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                NotifyError("User not found", "Could not resolve the user with the provided id.");
                return Response<object>(null);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(!result.Succeeded)
            {
                AddIdentityErrors(result);
            }
            return Response("Email verified");
        }

        [ProducesResponseType(typeof(ApiResult<ChangePasswordViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<ChangePasswordViewModel>), 400)]
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
                AddIdentityErrors(changePasswordResult);
            }

            return Response(model);
        }

        [ProducesResponseType(typeof(ApiResult<ForgotPasswordViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<ForgotPasswordViewModel>), 400)]
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> RequestPasswordForgottenMail([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                NotifyError("", "");
                return Response(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = GetPasswordResetCallbackUrl(user.Email, token);

            await _emailSender.SendResetPasswordAsync(user.Email, url);

            return Response(model);
        }

        [ProducesResponseType(typeof(ApiResult<ResetPasswordViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<ResetPasswordViewModel>), 400)]
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                NotifyError("", "");
                return Response(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if(!result.Succeeded)
            {
                AddIdentityErrors(result);
            }

            return Response(model);
        }

        private string GetVerificationCallbackUrl(string code)
        {
            return _emailVerificationCallbackUrl
                .Replace("{{CODE}}", System.Uri.EscapeDataString(code))
                .Replace("{{UID}}", System.Uri.EscapeDataString(_user.Id.ToString()));
        }

        private string GetPasswordResetCallbackUrl(string email,string token)
        {
            return _passwordResetCallbackUrl
                .Replace("{{EMAIL}}", System.Uri.EscapeDataString(email))
                .Replace("{{TOKEN}}", System.Uri.EscapeDataString(token));
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
