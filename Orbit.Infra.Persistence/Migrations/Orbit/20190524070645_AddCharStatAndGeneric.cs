using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    public partial class AddCharStatAndGeneric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    UpdateId = table.Column<Guid>(nullable: false),
                    IsStaff = table.Column<bool>(nullable: false),
                    PlayerId = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    GearScore = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    PlayTime = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Dexterity = table.Column<int>(nullable: false),
                    Stamina = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    Perin = table.Column<int>(nullable: false),
                    Penya = table.Column<int>(nullable: false),
                    RedChips = table.Column<int>(nullable: false),
                    EuphresiaCoins = table.Column<int>(nullable: false),
                    VotePoints = table.Column<int>(nullable: false),
                    DonateCoins = table.Column<int>(nullable: false),
                    BossKills = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenericObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    StatGroup = table.Column<string>(nullable: true),
                    StatName = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "GenericObjects");

            migrationBuilder.DropTable(
                name: "StatisticsEntries");
        }
    }
}
