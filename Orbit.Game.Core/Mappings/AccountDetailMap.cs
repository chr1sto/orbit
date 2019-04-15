using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    class AccountDetailMap : IEntityTypeConfiguration<AccountDetail>
    {
        public void Configure(EntityTypeBuilder<AccountDetail> builder)
        {
            builder.Property(x => x.AccountId)
                    .HasColumnType("varchar(32)")
                    .HasColumnName("account");
            builder.Property(x => x.GameCode)
                .HasColumnType("char(4)")
                .HasColumnName("gamecode");
            builder.Property(x => x.Tester)
                .HasColumnType("char(1)")
                .HasColumnName("tester");
            builder.Property(x => x.LoginAuthority)
                .HasColumnType("char(1)")
                .HasColumnName("m_chLoginAuthority");
            builder.Property(x => x.RegDate)
                .HasColumnType("datetime")
                .HasColumnName("regdate");
            builder.Property(x => x.BlockTime)
                .HasColumnType("char(8)")
                .HasColumnName("BlockTime");
            builder.Property(x => x.EndTime)
                .HasColumnType("char(8)")
                .HasColumnName("EndTime");
            builder.Property(x => x.WebTime)
                .HasColumnType("char(8)")
                .HasColumnName("WebTime");
            builder.Property(x => x.IsUse)
                .HasColumnType("char(1)")
                .HasColumnName("isuse");
            builder.Property(x => x.Secession)
                .HasColumnType("datetime")
                .HasColumnName("secession");
            builder.Property(x => x.Email)
                .HasColumnType("varchar(100)")
                .HasColumnName("email");
            builder.ToTable("ACCOUNT_TBL_DETAIL");
            builder.HasKey(x => x.AccountId);
        }
    }
}
