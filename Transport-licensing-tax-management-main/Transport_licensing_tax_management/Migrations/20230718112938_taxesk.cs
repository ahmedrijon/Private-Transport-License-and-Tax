using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transport_licensing_tax_management.Migrations
{
    public partial class taxesk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Taxes");

            migrationBuilder.AddColumn<long>(
                name: "PaymentsID",
                table: "Taxes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentsID",
                table: "Taxes");

            migrationBuilder.AddColumn<double>(
                name: "Fees",
                table: "Taxes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
