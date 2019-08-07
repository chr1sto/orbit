using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Design;
using Orbit.Domain.News;
using Orbit.Domain.Game.Models;
using Microsoft.Extensions.Logging;
using Orbit.Domain.GameCharacter;
using Orbit.Domain.Statistics;
using Orbit.Domain.Generic;
using Orbit.Domain.Transaction;

namespace Orbit.Infra.Persistence.Context
{
    public class OrbitContext : DbContext
    {
        private readonly IHostingEnvironment _env;
        private readonly ILoggerFactory _loggerFactory;

        public OrbitContext(IHostingEnvironment env)
        {
            _env = env;
        }

        public OrbitContext(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _env = env;
            _loggerFactory = loggerFactory;
        }

        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<GameAccount> GameAccounts { get; set; }
        public DbSet<ServiceStatus> ServiceStates { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<StatisticsEntry> StatisticsEntries { get; set; }
        public DbSet<GenericObject> GenericObjects { get; set; }
        public DbSet<Transaction> Transactions { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            if(_loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
        }
    }
}
