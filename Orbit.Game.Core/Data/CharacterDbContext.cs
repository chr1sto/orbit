using Microsoft.EntityFrameworkCore;
using Orbit.Game.Core.Mappings;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Data
{
    public class CharacterDbContext : DbContext
    {
        public virtual DbSet<Character> Characters { get; set; }
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CharacterMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
