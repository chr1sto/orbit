using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class GuildMemberMap : IEntityTypeConfiguration<GuildMember>
    {
        public void Configure(EntityTypeBuilder<GuildMember> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnType("char(7)")
                .HasColumnName("m_idPlayer");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.GuildId)
                .HasColumnType("char(6)")
                .HasColumnName("m_idGuild");
            builder.Property(x => x.MemberLevel)
                .HasColumnType("int")
                .HasColumnName("m_nMemberLv");
            builder.Property(x => x.WinCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.LooseCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.SurrenderCount)
                .HasColumnType("int")
                .HasColumnName("m_nWin");
            builder.Property(x => x.Alias)
                .HasColumnType("varchar(20)")
                .HasColumnName("m_szAlias");
            builder.ToTable("GUILD_MEMBER_TBL");
        }
    }
}
