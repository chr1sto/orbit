using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service
{
    public class Program
    {
        private static CancellationTokenSource pollingCancellationTokenSource;
        private static CancellationToken pollingCancellationToken;

        private static CancellationTokenSource updateLoopCancellationTokenSource;
        private static CancellationToken updateLoopCancellationToken;

        private static TimeSpan serviceStatusUpdateInterval = new TimeSpan(0, 0, 20);

        private static Timer eventPollingTimer;
        private static Timer serviceUpdateTimer;
        private static Timer updateCharsTimer;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            var bootstrapper = new InjectionBootstrapper();
            bootstrapper.ConfigureServices(services);

            services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(configuration["DefaultConnection"]));
            services.AddDbContext<CharacterDbContext>(options => options.UseSqlServer(configuration["CHARACTER_DBF"]));

            services.AddLogging(e => e.AddConsole());

            services.AddSingleton<IConfiguration>(configuration);

            var serviceProvider = services.BuildServiceProvider();

            //Authenticate
            if(Authenticate(serviceProvider, configuration).Result)
            {
                eventPollingTimer = new System.Threading.Timer(
                    async (e) => {
                        var webEventHandler = serviceProvider.GetService<IWebEventHandler>();
                        var logger = serviceProvider.GetService<ILogger<Program>>();
                        try
                        {
                            await webEventHandler.PollAndHandleEvents();
                        }
                        catch (Exception ex)
                        {
                            logger.LogCritical("Unable to poll events:\nException:\n{0}", ex.Message);
                        }
                        eventPollingTimer.Change(5000, Timeout.Infinite);
                    }
                    , null
                    , 5000
                    , Timeout.Infinite);

                serviceUpdateTimer = new System.Threading.Timer(
                    async (e) =>
                    {
                        var service = serviceProvider.GetRequiredService<IServiceStatusService>();
                        service.Update();
                        serviceUpdateTimer.Change(120000, Timeout.Infinite);
                    }
                    , null
                    , 120000
                    , Timeout.Infinite
                    );

                updateCharsTimer = new System.Threading.Timer(
                    async (e) =>
                    {
                        var service = serviceProvider.GetRequiredService<IGameCharacterService>();
                        service.UpdateAll();
                        updateCharsTimer.Change(120000, Timeout.Infinite);
                    }
                    , null
                    , 5000//120000
                    , Timeout.Infinite
                    );
            }

            Console.ReadLine();
        }

        static async Task<bool> Authenticate(ServiceProvider serviceProvider, IConfiguration configuration)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var accountClient = new AccountClient(configuration["BASE_API_PATH"], httpClient);
            var result = await accountClient.LoginAsync(new LoginViewModel()
            {
                Email = configuration["Credentials:Email"],
                Password = configuration["Credentials:Password"],
                RememberMe = true
            });

            if(result?.Errors != null)
            {
                logger.LogError("Unable to Authenticate with given credentials! Please check your appsettings.json");
                return false;
            }

            if(result.Data == null)
            {
                logger.LogError("An uknown Error occured while trying to authenticate!");
                return false;
            }

            var token = (string)result.Data;

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return true;
        }
            
    }
}
