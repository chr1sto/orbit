using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class CombatJoinPlayerMap : IEntityTypeConfiguration<CombatJoinPlayer>
    {
        public void Configure(EntityTypeBuilder<CombatJoinPlayer> builder)
        {
            builder.Property(x => x.CombatId)
                .HasColumnType("int")
                .HasColumnName("CombatID");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.GuildId)
                .HasColumnType("char(6)")
                .HasColumnName("GuildID");
            builder.Property(x => x.PlayerId)
                .HasColumnType("char(7)")
                .HasColumnName("PlayerID");
            builder.Property(x => x.Status)
                .HasColumnType("varchar(3)")
                .HasColumnName("Status");           
            builder.Property(x => x.Reward)
                .HasColumnType("bigint")
                .HasColumnName("Reward");
            builder.Property(x => x.Point)
                .HasColumnType("int")
                .HasColumnName("Point");
            builder.Property(x => x.RewardDate)
                .HasColumnType("datetime")
                .HasColumnName("RewardDate");
            builder.ToTable("tblCombatInfo");
        }
    }
}
