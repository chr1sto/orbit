using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    public partial class AddServiceStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Service = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceStates");
        }
    }
}
