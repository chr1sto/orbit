﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orbit.Infra.Persistence.Context;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    [DbContext(typeof(OrbitContext))]
    partial class OrbitContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Orbit.Domain.Game.Models.GameAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account");

                    b.Property<string>("Alias");

                    b.Property<Guid>("UserID");

                    b.HasKey("Id");

                    b.ToTable("GameAccounts");
                });

            modelBuilder.Entity("Orbit.Domain.Game.Models.ServiceStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Service");

                    b.Property<int>("State");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("ServiceStates");
                });

            modelBuilder.Entity("Orbit.Domain.News.NewsPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption");

                    b.Property<string>("Content");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ForumPostUrl");

                    b.Property<string>("ImageUrlBanner");

                    b.Property<string>("ImageUrlBigTile");

                    b.Property<string>("ImageUrlSmallTile");

                    b.Property<bool>("Public");

                    b.Property<string>("Tags");

                    b.HasKey("Id");

                    b.ToTable("NewsPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
