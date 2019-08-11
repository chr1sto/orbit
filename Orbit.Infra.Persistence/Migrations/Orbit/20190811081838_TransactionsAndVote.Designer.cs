﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orbit.Infra.Persistence.Context;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    [DbContext(typeof(OrbitContext))]
    [Migration("20190811081838_TransactionsAndVote")]
    partial class TransactionsAndVote
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Orbit.Domain.GameCharacter.Character", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account");

                    b.Property<int>("BossKills");

                    b.Property<string>("Class");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Dexterity");

                    b.Property<long>("DonateCoins");

                    b.Property<long>("EuphresiaCoins");

                    b.Property<int>("GearScore");

                    b.Property<int>("Intelligence");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsStaff");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<long>("Penya");

                    b.Property<long>("Perin");

                    b.Property<int>("PlayTime");

                    b.Property<string>("PlayerId");

                    b.Property<long>("RedChips");

                    b.Property<int>("Stamina");

                    b.Property<int>("Strength");

                    b.Property<Guid>("UpdateId");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<long>("VotePoints");

                    b.HasKey("Id");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Orbit.Domain.Generic.GenericObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Type");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.ToTable("GenericObjects");
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

            modelBuilder.Entity("Orbit.Domain.Statistics.StatisticsEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("Start");

                    b.Property<string>("StatGroup");

                    b.Property<string>("StatName");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.HasKey("Id");

                    b.ToTable("StatisticsEntries");
                });

            modelBuilder.Entity("Orbit.Domain.Transaction.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<string>("Currency");

                    b.Property<DateTime>("Date");

                    b.Property<string>("IpAddress");

                    b.Property<string>("Reason");

                    b.Property<string>("RemoteAddress");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
