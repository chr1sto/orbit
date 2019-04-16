using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Design;
using Orbit.Domain.News;
using Orbit.Domain.Game.Models;

namespace Orbit.Infra.Persistence.Context
{
    public class OrbitContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public OrbitContext(IHostingEnvironment env)
        {
            _env = env;
        }

        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<GameAccount> GameAccounts { get; set; }
        public DbSet<ServiceStatus> ServiceStates { get; set; }

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
        }
    }
}
