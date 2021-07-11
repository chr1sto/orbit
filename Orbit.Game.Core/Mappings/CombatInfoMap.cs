using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class CombatInfoMap : IEntityTypeConfiguration<CombatInfo>
    {
        public void Configure(EntityTypeBuilder<CombatInfo> builder)
        {
            builder.Property(x => x.CombatId)
                .HasColumnType("int")
                .HasColumnName("CombatID");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.Status)
                .HasColumnType("varchar(3)")
                .HasColumnName("Status");
            builder.Property(x => x.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("StartDate");
            builder.Property(x => x.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("EndDate");
            builder.Property(x => x.Comment)
                .HasColumnType("varchar(1000)")
                .HasColumnName("Comment");
            builder.Property(x => x.Seq)
                .HasColumnType("bigint")
                .HasColumnName("SEQ");
            builder.ToTable("tblCombatInfo");
        }
    }
}
