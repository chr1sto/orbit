using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Orbit.Api.Misc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.CrossCutting.Identity.Extensions;
using Orbit.Infra.CrossCutting.Identity.Models;
using Orbit.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Orbit.Infra.CrossCutting.Identity.Services;
using X.PagedList;

namespace Orbit.Api.Controllers
{
    [Route("account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly IGameAccountAppService _gameAccountAppService;
        private readonly string _emailVerificationCallbackUrl;
        private readonly string _passwordResetCallbackUrl;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IGameAccountAppService gameAccountAppService,
            INotificationHandler<DomainNotification> notifications, 
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _configuration = configuration;
            _gameAccountAppService = gameAccountAppService;
            _emailSender = emailSender;
            _env = env;

            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            _emailVerificationCallbackUrl = config["EMAIL_VERIFICATION_CALLBACK_URL"];
            _passwordResetCallbackUrl = config["PASSWORD_RESET_CALLBACK_URL"];
        }

        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<LoginViewModel>), 400)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if(user == null)
            {
                NotifyError("Invalid credentials!", "You either provided a wrong password or an account with this email does not exist.");
                return Response(loginViewModel);
            }

            if(!user.EmailConfirmed)
            {
                NotifyError("MAIL_NOT_VERIFIED", "You need to verify your email in order to be able to login!");
                return Response(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, true);
            if(!result.Succeeded)
            {
                NotifyError("Invalid credentials!", "You either provided a wrong password or an account with this email does not exist.");
                return Response(loginViewModel);
            }

            _logger.LogInformation(1, "User logged in.");

            return await GetJwtToken(user);
        }

        [ProducesResponseType(typeof(ApiResult<RegisterViewModel>), 200)]
        [ProducesResponseType(typeof(ApiResult<RegisterViewModel>), 400)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(registerViewModel);
            }

            var user = new ApplicationUser
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if(result.Succeeded)
            {
                try
                {
                    _gameAccountAppService.Create(new Application.ViewModels.GameAccountViewModel()
                    {
                        Alias = "MyGameAccount",
                        Account = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower(),
                        Id = Guid.NewGuid()
                    });
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(3, "Something went terribly wrong when creating the default GameAccount:\n",ex.Message);
                }

                //await _signInManager.SignInAsync(user, false);
                //Move this to a Service in Infra.CrossCutting.Identity!
                user = await _userManager.FindByEmailAsync(user.Email);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var email = user.Email;
                var callback = GetVerificationCallbackUrl(code,user);
                await _emailSender.SendEmailConfirmationAsync(email, callback);
                _logger.LogInformation(3, "User created a new account with password");

                return Response("Verification Mail sent.");

                //return await GetJwtToken(user);
                //return Response(registerViewModel);
            }

            AddIdentityErrors(result);
            return Response(registerViewModel);
        }

        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<ApplicationUser>>>), 200)]
        [ProducesResponseType(typeof(ApiResult<PagedResultData<IEnumerable<ApplicationUser>>>), 400)]
        [Authorize(Roles="Administrator,Gamemaster,Developer")]
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] string searchText = "")
        {
            IList<ApplicationUser> users;
            int recCount = 0;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                users = await _userManager.Users.OrderBy(o => o.Email).Skip(index * count).Take(count).ToListAsync();
                recCount = _userManager.Users.Count();
            }
            else
            {
                users = await _userManager.Users.Where(u => u.Email.Contains(searchText)).OrderBy(o => o.Email).Skip(index * count).Take(count).ToListAsync();
                recCount = _userManager.Users.Where(u => u.Email.Contains(searchText)).Count();
            }
            var pagedApiResult = new PagedResultData<IEnumerable<ApplicationUser>>(users, recCount, index, count);
            return Response(pagedApiResult);
        }

        [ProducesResponseType(typeof(ApiResult<ApplicationUser>),200)]
        [ProducesResponseType(typeof(ApiResult<ApplicationUser>), 400)]
        [Authorize(Roles = "Administrator,Gamemaster,Developer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return Response(result);
        }

        private async Task<IActionResult> GetJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
            var roles = await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.Now.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.Id?.ToString() ?? ""),
                    new Claim(ClaimTypes.Name,user.UserName ?? ""),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
                }),
                Expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExprieDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtIssuer"],
                Audience = _configuration["JwtIssuer"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Response(tokenHandler.WriteToken(token));
        }

        private string GetVerificationCallbackUrl(string code, ApplicationUser user)
        {
            return _emailVerificationCallbackUrl
                .Replace("{{CODE}}", System.Uri.EscapeDataString(code))
                .Replace("{{UID}}", System.Uri.EscapeDataString(user.Id.ToString()));
        }
    }
}
