using Microsoft.Extensions.DependencyInjection;
using Orbit.Game.Core.Handlers;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Orbit.Game.Service
{
    public class InjectionBootstrapper
    {
        public InjectionBootstrapper()
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGameAccountService, GameAccountService>();
            services.AddSingleton<IWebEventService, WebEventService>();
            services.AddSingleton<IWebEventHandler, WebEventHandler>();
            services.AddSingleton<IServiceStatusService, ServiceStatusService>();
            services.AddSingleton<IGameCharacterService, GameCharacterService>();
            services.AddSingleton<HttpClient>();
        }
    }
}
