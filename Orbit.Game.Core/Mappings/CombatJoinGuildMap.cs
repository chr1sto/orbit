using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class CombatJoinGuildMap : IEntityTypeConfiguration<CombatJoinGuild>
    {
        public void Configure(EntityTypeBuilder<CombatJoinGuild> builder)
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
            builder.Property(x => x.Status)
                .HasColumnType("varchar(3)")
                .HasColumnName("Status");
            builder.Property(x => x.ApplyDate)
                .HasColumnType("datetime")
                .HasColumnName("ApplyDate");
            builder.Property(x => x.CombatFee)
                .HasColumnType("bigint")
                .HasColumnName("CombatFee");
            builder.Property(x => x.ReturnCombatFee)
                .HasColumnType("bigint")
                .HasColumnName("ReturnCombatFee");
            builder.Property(x => x.Reward)
                .HasColumnType("bigint")
                .HasColumnName("Reward");
            builder.Property(x => x.Point)
                .HasColumnType("int")
                .HasColumnName("Point");
            builder.Property(x => x.RewardDate)
                .HasColumnType("datetime")
                .HasColumnName("RewardDate");
            builder.Property(x => x.CancelDate)
                .HasColumnType("datetime")
                .HasColumnName("RewardDate");
            builder.Property(x => x.Seq)
                .HasColumnType("bigint")
                .HasColumnName("SEQ");
            builder.ToTable("tblCombatJoinGuild");
        }
    }
}
