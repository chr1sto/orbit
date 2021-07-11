using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ImageUrlSmallTile = table.Column<string>(nullable: true),
                    ImageUrlBigTile = table.Column<string>(nullable: true),
                    ImageUrlBanner = table.Column<string>(nullable: true),
                    ForumPostUrl = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Public = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsPosts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsPosts");
        }
    }
}
