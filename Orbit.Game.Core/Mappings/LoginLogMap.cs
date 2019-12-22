using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orbit.Game.Core.Models.LoggingDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Game.Core.Mappings
{
    public class LoginLogMap : IEntityTypeConfiguration<LoginLog>
    {
        public void Configure(EntityTypeBuilder<LoginLog> builder)
        {
            var converter = new ValueConverter<DateTime, string>(a => (a - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString(),b => new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(int.Parse(b)));

            builder.Property(x => x.PlayerId)
                .HasColumnType("char(7)")
                .HasColumnName("m_idPlayer");
            builder.Property(x => x.ServerIndex)
                .HasColumnType("char(2)")
                .HasColumnName("serverindex");
            builder.Property(x => x.WorldId)
                .HasColumnType("int)")
                .HasColumnName("dwWorldId");
            builder.Property(x => x.StartTime)
                .HasColumnType("char(14)")
                .HasColumnName("StartTime")
                .HasConversion(converter);
            builder.Property(x => x.EndTime)
                .HasColumnType("char(14)")
                .HasColumnName("EndTime")
                .HasConversion(converter);
            builder.Property(x => x.TotalPlayTime)
                .HasColumnType("int)")
                .HasColumnName("TotalPlayTime");
            builder.Property(x => x.Gold)
                .HasColumnType("int")
                .HasColumnName("m_dwGold");
            builder.Property(x => x.RemoteIp)
                .HasColumnType("varchar(32)")
                .HasColumnName("remoteIP");
            builder.Property(x => x.CharLevel)
                .HasColumnType("int)")
                .HasColumnName("CharLevel");
            builder.Property(x => x.Job)
                .HasColumnType("int")
                .HasColumnName("Job");
            builder.Property(x => x.State)
                .HasColumnType("tinyint")
                .HasColumnName("State");
            builder.Property(x => x.SEQ)
                .HasColumnType("bigint")
                .HasColumnName("SEQ");
            builder.ToTable("LOG_LOGIN_TBL");
            builder.HasKey(o => new
            {
                o.PlayerId,
                o.ServerIndex,
                o.SEQ
            });
        }
    }
}
