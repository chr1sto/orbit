using Microsoft.EntityFrameworkCore.Migrations;

namespace Orbit.Infra.Persistence.Migrations.Orbit
{
    public partial class TransactionsWithdrawal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Target",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetInfo",
                table: "Transactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TargetInfo",
                table: "Transactions");
        }
    }
}
