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
        private static Timer eventPollingTimer;
        private static Timer serviceUpdateTimer;
        private static Timer updateCharsTimer;
        private static Timer processTransactionsTimer;

        private static bool eventPollingRunning = false;
        private static bool serviceUpdateRunning = false;
        private static bool updateCharsRunning = false;
        private static bool processTransactionsRunning = false;

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

            bool authenticated = false;

            try
            {
                authenticated = Authenticate(serviceProvider, configuration).Result;
            }
            catch(Exception ex)
            {
                var logger = serviceProvider.GetService<ILogger<Program>>();
                logger.LogCritical("Could not authenticate!!!\nException:\n{0}", ex.Message);
            }

            if (authenticated)
            {
                eventPollingTimer = new System.Threading.Timer(
                    async (e) => {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            if (!eventPollingRunning)
                            {
                                eventPollingRunning = true;
                                var webEventHandler = scope.ServiceProvider.GetService<IWebEventHandler>();
                                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                                try
                                {
                                    await webEventHandler.PollAndHandleEvents();
                                }
                                catch (Exception ex)
                                {
                                    logger.LogCritical("Unable to poll events:\nException:\n{0}", ex.Message);
                                }
                                finally
                                {
                                    eventPollingRunning = false;
                                    eventPollingTimer.Change(5000, Timeout.Infinite);
                                }
                            }

                        }

                    }
                    , null
                    , 5000
                    , Timeout.Infinite);

                serviceUpdateTimer = new System.Threading.Timer(
                    async (e) =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            if (!serviceUpdateRunning)
                            {
                                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                                try
                                {
                                    serviceUpdateRunning = true;
                                    var service = scope.ServiceProvider.GetRequiredService<IServiceStatusService>();
                                    service.Update();
                                }
                                catch (Exception ex)
                                {
                                    logger.LogCritical("Unable to update service states:\nException:\n{0}", ex.Message);
                                }
                                finally
                                {
                                    serviceUpdateRunning = false;
                                    serviceUpdateTimer.Change(120000, Timeout.Infinite);
                                }
                            }
                        }

                    }
                    , null
                    , 120000
                    , Timeout.Infinite
                    );

                updateCharsTimer = new System.Threading.Timer(
                    async (e) =>
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            if (!updateCharsRunning)
                            {
                                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                                try
                                {
                                    updateCharsRunning = true;
                                    var service = scope.ServiceProvider.GetRequiredService<IGameCharacterService>();
                                    service.UpdateAll();
                                }
                                catch (Exception ex)
                                {
                                    logger.LogCritical("Unable to update characters:\nException:\n{0}", ex.Message);
                                }
                                finally
                                {
                                    updateCharsRunning = false;
                                    updateCharsTimer.Change(120000, Timeout.Infinite);
                                }
                            }
                        }

                    }
                    , null
                    , 5000//120000
                    , Timeout.Infinite
                    );

                processTransactionsTimer = new System.Threading.Timer(
                    async (e) =>
                    {
                        if(!processTransactionsRunning)
                        {
                            using(var scope = serviceProvider.CreateScope())
                            {
                                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                                try
                                {
                                    processTransactionsRunning = true;
                                    var service = scope.ServiceProvider.GetRequiredService<IProcessTransactionsService>();
                                    await service.Process();
                                }
                                catch (Exception ex)
                                {
                                    logger.LogCritical("Unable to process transactions:\nException:\n{0}", ex.Message);
                                }
                                finally
                                {
                                    processTransactionsRunning = false;
                                    processTransactionsTimer.Change(5000, Timeout.Infinite);
                                }
                            }

                        }
                    }
                    , null
                    , 5000
                    , Timeout.Infinite
                    );
            }

            Console.ReadLine();
        }

        static async Task<bool> Authenticate(ServiceProvider serviceProvider, IConfiguration configuration)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var accountClient = new AccountClient(httpClient);
            accountClient.BaseUrl = configuration["BASE_API_PATH"];
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
