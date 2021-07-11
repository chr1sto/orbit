using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class SendItemMap : IEntityTypeConfiguration<SendItem>
    {
        public void Configure(EntityTypeBuilder<SendItem> builder)
        {
            builder.Property(x => x.PlayerId)
                .HasColumnType("char(7)")
                .HasColumnName("m_idPlayer");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.ItemName)
                .HasColumnType("varchar(32)")
                .HasColumnName("Item_Name");
            builder.Property(x => x.ItemCount)
                .HasColumnType("int")
                .HasColumnName("Item_count");
            builder.Property(x => x.SenderId)
                .HasColumnType("char(7)")
                .HasColumnName("idSender");
            builder.Property(x => x.AbilityObtion)
                .HasColumnType("int")
                .HasColumnName("m_nAbilityOption");
            builder.Property(x => x.ItemResist)
                .HasColumnType("int")
                .HasColumnName("m_bItemResist");
            builder.Property(x => x.ResistAbilityOption)
                .HasColumnType("int")
                .HasColumnName("m_nResistAbilityOption");
            builder.Property(x => x.Charged)
                .HasColumnType("int")
                .HasColumnName("m_bCharged");
            builder.Property(x => x.PiercedSize)
                .HasColumnType("int")
                .HasColumnName("nPiercedSize");
            builder.Property(x => x.DwItemId0)
                .HasColumnType("int")
                .HasColumnName("adwItemId0");
            builder.Property(x => x.DwItemId1)
                .HasColumnType("int")
                .HasColumnName("adwItemId1");
            builder.Property(x => x.DwItemId2)
                .HasColumnType("int")
                .HasColumnName("adwItemId2");
            builder.Property(x => x.DwItemId3)
                .HasColumnType("int")
                .HasColumnName("adwItemId3");
            builder.Property(x => x.KeepTime)
                .HasColumnType("bigint")
                .HasColumnName("m_dwKeepTime");
            builder.Property(x => x.ItemFlag)
                .HasColumnType("int")
                .HasColumnName("ItemFlag");
            builder.Property(x => x.ReceiveDate)
                .HasColumnType("datetime")
                .HasColumnName("ReceiveDt");
            builder.Property(x => x.Key)
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasColumnName("m_nNo");
            builder.ToTable("ITEM_SEND_TBL");
            builder.HasKey(x => x.Key);
        }
    }
}
