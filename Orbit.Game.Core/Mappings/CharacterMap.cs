using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    class CharacterMap : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.Property(x => x.IdPlayer)
                .HasColumnType("char(7)")
                .HasColumnName("m_idPlayer");
            builder.Property(x => x.Account)
                .HasColumnType("varchar(32)")
                .HasColumnName("account");
            builder.Property(x => x.Name)
                .HasColumnType("varchar(32")
                .HasColumnName("m_szName");
            builder.Property(x => x.Gold)
                .HasColumnType("int")
                .HasColumnName("m_dwGold");
            builder.Property(x => x.Job)
                .HasColumnType("int")
                .HasColumnName("m_nJob");
            builder.Property(x => x.Str)
                .HasColumnType("int")
                .HasColumnName("m_nStr");
            builder.Property(x => x.Sta)
                .HasColumnType("int")
                .HasColumnName("m_nSta");
            builder.Property(x => x.Dex)
                .HasColumnType("int")
                .HasColumnName("m_nDex");
            builder.Property(x => x.Int)
                .HasColumnType("int")
                .HasColumnName("m_nInt");
            builder.Property(x => x.Level)
                .HasColumnType("int")
                .HasColumnName("m_nLevel");
            builder.Property(x => x.Authority)
                .HasColumnType("char(1)")
                .HasColumnName("m_chAuthority");
            builder.Property(x => x.TotalPlayTime)
                .HasColumnType("int")
                .HasColumnName("TotalPlayTime");
            builder.Property(x => x.Perin)
                .HasColumnType("bigint")
                .HasColumnName("m_nPerin");
            builder.Property(x => x.Farm)
                .HasColumnType("bigint")
                .HasColumnName("m_nFarm");
            builder.Property(x => x.Donate)
                .HasColumnType("bigint")
                .HasColumnName("m_nDonate");
            builder.Property(x => x.Vote)
                .HasColumnType("bigint")
                .HasColumnName("m_nVote");
            builder.Property(x => x.Chips)
                .HasColumnType("bigint")
                .HasColumnName("m_nChips");
            builder.Property(x => x.BossKills)
                .HasColumnType("int")
                .HasColumnName("m_nBossKills");
            builder.Property(x => x.GearScore)
                .HasColumnType("int")
                .HasColumnName("m_nGearScore");
            builder.Property(x => x.IsBlock)
                .HasColumnType("char(1)")
                .HasColumnName("isblock");
            builder.ToTable("CHARACTER_TBL");
            builder.HasKey(x => x.IdPlayer);
        }
    }
}
