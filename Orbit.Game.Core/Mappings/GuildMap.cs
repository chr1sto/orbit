using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class GuildMap : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnType("char(6)")
                .HasColumnName("m_idGuild");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.Name)
                .HasColumnType("varchar(16)")
                .HasColumnName("m_szGuild");
            builder.Property(x => x.Level)
                .HasColumnType("int")
                .HasColumnName("m_nLevel");
            builder.Property(x => x.WinCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.LooseCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.SurrenderCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.ToTable("GUILD_TBL");
            builder.HasKey(x => x.Id);
        }
    }
}
