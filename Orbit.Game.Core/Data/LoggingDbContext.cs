using Microsoft.EntityFrameworkCore;
using Orbit.Game.Core.Mappings;
using Orbit.Game.Core.Models.LoggingDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Data
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<CharacterDbContext> options) : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        public virtual DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LoginLogMap());
        }
    }
}
