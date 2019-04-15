using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class AccountPlayTimeMap : IEntityTypeConfiguration<AccountPlayTime>
    {
        public void Configure(EntityTypeBuilder<AccountPlayTime> builder)
        {
            builder.Property(x => x.AccountId)
                .HasColumnType("varchar(32)")
                .HasColumnName("Account");
            builder.Property(x => x.PlayDate)
                .HasColumnType("int")
                .HasColumnName("PlayDate");
            builder.Property(x => x.PlayTime)
                .HasColumnType("int")
                .HasColumnName("PlayTime");
            builder.ToTable("AccountPlay");
            builder.HasKey(x => x.AccountId);
        }
    }
}
