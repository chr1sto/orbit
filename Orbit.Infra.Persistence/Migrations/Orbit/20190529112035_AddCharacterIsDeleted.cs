using Microsoft.EntityFrameworkCore.Migrations;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    public partial class AddCharacterIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Characters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Characters");
        }
    }
}
