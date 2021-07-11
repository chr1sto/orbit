using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service.Services
{
    public sealed class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationService> _logger;

        private string token = null;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public AuthenticationService(HttpClient httpClient, IConfiguration configuration, ILogger<AuthenticationService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        private bool authenticated = false;
        public async Task<bool> Authenticate()
        {

            var handler = new JwtSecurityTokenHandler();
            if (token == null || handler.ReadJwtToken(token).ValidTo < DateTime.Now)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    if(token == null || handler.ReadJwtToken(token).ValidTo < DateTime.Now)
                    {
                        var accountClient = new AccountClient(_httpClient);
                        accountClient.BaseUrl = _configuration["BASE_API_PATH"];
                        var result = await accountClient.LoginAsync(new LoginViewModel()
                        {
                            Email = _configuration["Credentials:Email"],
                            Password = _configuration["Credentials:Password"],
                            RememberMe = true
                        });

                        if (result?.Errors != null)
                        {
                            _logger.LogError("Unable to Authenticate with given credentials! Please check your appsettings.json");
                            return false;
                        }

                        if (result.Data == null)
                        {
                            _logger.LogError("An uknown Error occured while trying to authenticate!");
                            return false;
                        }

                        token = (string)result.Data;
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                        authenticated = true;
                    }                    
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return authenticated;

        }
    }
}
