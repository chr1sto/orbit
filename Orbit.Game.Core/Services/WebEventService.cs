using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orbit.Domain.Core.Events;
using Orbit.Game.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Services
{
    public class WebEventService : IWebEventService
    {
        private readonly ILogger<WebEventService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WebEventService(ILogger<WebEventService> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<ICollection<StoredEvent>> GetUnhandled()
        {
            var client = new GameEventClient(_configuration["BASE_API_PATH"], _httpClient);
            var result = await client.GameEventsGetAsync();
            var data = result.Data;
            return data.ToList();
        }
         
        public async Task<bool> SetHandled(Guid id)
        {
            var client = new GameEventClient(_configuration["BASE_API_PATH"], _httpClient);
            await client.GameEventsPatchAsync(id);
            return true;
        }
    }
}
