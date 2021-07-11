using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.AccountId)
                    .HasColumnType("varchar(32)")
                    .HasColumnName("account");
            builder.Property(x => x.Password)
                .HasColumnType("varchar(32)")
                .HasColumnName("password");
            builder.Property(x => x.IsUse)
                .HasColumnType("char(1)")
                .HasColumnName("isuse");
            builder.Property(x => x.Member)
                .HasColumnType("char(1)")
                .HasColumnName("member");
            builder.Property(x => x.IdNo1)
                .HasColumnType("varchar(32)")
                .HasColumnName("id_no1");
            builder.Property(x => x.IdNo2)
                .HasColumnType("varchar(32)")
                .HasColumnName("id_no2");
            builder.Property(x => x.RealName)
                .HasColumnType("char(1)")
                .HasColumnName("realname");
            builder.Property(x => x.Reload)
                .HasColumnType("char(1)")
                .HasColumnName("reload");
            builder.Property(x => x.OldPassword)
                .HasColumnType("varchar(32)")
                .HasColumnName("OldPassword");
            builder.Property(x => x.TempPassword)
                .HasColumnType("varchar(32)")
                .HasColumnName("TempPassword");
            builder.Property(x => x.Cash)
                .HasColumnType("int")
                .HasColumnName("cash");
            builder.Property(x => x.UserId)
                .HasColumnType("nvarchar(36)")
                .HasColumnName("userId");
            builder.ToTable("ACCOUNT_TBL");
            builder.HasKey(x => x.AccountId);
        }
    }
}
