using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class GuildWarMap : IEntityTypeConfiguration<GuildWar>
    {
        public void Configure(EntityTypeBuilder<GuildWar> builder)
        {
            builder.Property(x => x.GuildId)
                .HasColumnType("char(6)")
                .HasColumnName("m_idGuild");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.WarId)
                .HasColumnType("int")
                .HasColumnName("m_idWar");
            builder.Property(x => x.FGuildId)
                .HasColumnType("char(6)")
                .HasColumnName("f_idGuild");
            builder.Property(x => x.DeathCount)
                .HasColumnType("int")
                .HasColumnName("m_nDeath");
            builder.Property(x => x.SurrenderCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.AbsentCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.FDeathCount)
                .HasColumnType("int")
                .HasColumnName("f_nDeath");
            builder.Property(x => x.FSurrenderCount)
                .HasColumnType("int")
                .HasColumnName("f_nWin");
            builder.Property(x => x.FAbsentCount)
                .HasColumnType("int")
                .HasColumnName("f_nWin");
            builder.ToTable("GUILD_WAR_TBL");
        }
    }
}
