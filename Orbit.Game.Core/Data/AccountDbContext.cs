using Microsoft.EntityFrameworkCore;
using Orbit.Game.Core.Mappings;
using Orbit.Game.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Data
{
    public class AccountDbContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<AccountPlayTime> AccountPlayTimes { get; set; }
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new AccountDetailMap());
            modelBuilder.ApplyConfiguration(new AccountPlayTimeMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
