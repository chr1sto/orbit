using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Services;
using Orbit.Game.Service.Services;
using Orbit.Game.Service.Workers;
using Serilog;

namespace Orbit.Game.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File("logs\\logs-{date}.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddTransient<IWebEventService, WebEventService>();
                    services.AddTransient<IGameAccountService, GameAccountService>();
                    services.AddTransient<IServiceStatusService, ServiceStatusService>();
                    services.AddTransient<IGameCharacterService, GameCharacterService>();
                    services.AddTransient<IProcessTransactionsService, ProcessTransactionsService>();
                    services.AddTransient<IStatisticsService, StatisticsService>();

                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<AuthenticationService>();
                    services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(hostContext.Configuration["DefaultConnection"]));
                    services.AddDbContext<CharacterDbContext>(options => options.UseSqlServer(hostContext.Configuration["CHARACTER_DBF"]));
                    //services.AddDbContext<LoggingDbContext>(options => options.UseSqlServer(hostContext.Configuration["LOGGING_DBF"]));

                    services.AddHostedService<WebEventWorker>();
                    services.AddHostedService<StatusWorker>();
                    services.AddHostedService<TransactionsWorker>();
                    services.AddHostedService<CharacterWorker>();
                    services.AddHostedService<StatisticsWorker>();
                });
    }
}
