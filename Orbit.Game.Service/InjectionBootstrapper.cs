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
            services.AddTransient<IGameAccountService, GameAccountService>();
            services.AddTransient<IWebEventService, WebEventService>();
            services.AddTransient<IWebEventHandler, WebEventHandler>();
            services.AddTransient<IServiceStatusService, ServiceStatusService>();
            services.AddTransient<IGameCharacterService, GameCharacterService>();
            services.AddTransient<IProcessTransactionsService, ProcessTransactionsService>();
            services.AddSingleton<HttpClient>();
        }
    }
}
