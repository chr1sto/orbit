using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Game.Service
{
    public class Program
    {
        private static CancellationTokenSource pollingCancellationTokenSource;
        private static CancellationToken pollingCancellationToken;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            var bootstrapper = new InjectionBootstrapper();
            bootstrapper.ConfigureServices(services);

            services.AddDbContext<AccountDbContext>(options => 
                options.UseSqlServer(
                configuration["DefaultConnection"]));

            services.AddLogging(e => e.AddConsole());

            services.AddSingleton<IConfiguration>(configuration);

            var serviceProvider = services.BuildServiceProvider();

            //Event Polling
            pollingCancellationTokenSource = new CancellationTokenSource();
            pollingCancellationToken = pollingCancellationTokenSource.Token;
            var pollingBackgroundTask = Task.Run(() =>
            {
                RunPollingTask(serviceProvider);
            }, pollingCancellationToken);

            Console.ReadLine();
        }

        static async void RunPollingTask(ServiceProvider serviceProvider)
        {
            var webEventHandler = serviceProvider.GetService<IWebEventHandler>();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            while (!pollingCancellationToken.IsCancellationRequested)
            {
                try
                {
                    await webEventHandler.PollAndHandleEvents();
                }
                catch(Exception ex)
                {
                    logger.LogCritical("Unable to poll events:\nException:\n{0}",ex.Message);
                }

                Thread.Sleep(5000);
            }
        }
            
    }
}
