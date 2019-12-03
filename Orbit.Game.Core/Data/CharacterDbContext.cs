﻿using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<SendItem> SendItems { get; set; }
        public virtual DbQuery<Guild> Guilds { get; set; }
        public virtual DbQuery<GuildMember> GuildMembers { get; set; }
        public virtual DbQuery<GuildWar> GuildWars { get; set; }
        public virtual DbQuery<CombatInfo> CombatInfos { get; set; }
        public virtual DbQuery<CombatJoinPlayer> CombatJoinPlayers { get; set; }
        public virtual DbQuery<CombatJoinGuild> GetCombatJoinGuilds { get; set; }
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CharacterMap());
            modelBuilder.ApplyConfiguration(new SendItemMap());
            modelBuilder.ApplyConfiguration(new GuildMap());
            modelBuilder.ApplyConfiguration(new GuildMemberMap());
            modelBuilder.ApplyConfiguration(new GuildWarMap());
            modelBuilder.ApplyConfiguration(new CombatInfoMap());
            modelBuilder.ApplyConfiguration(new CombatJoinPlayerMap());
            modelBuilder.ApplyConfiguration(new CombatJoinGuildMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
