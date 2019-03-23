using Microsoft.EntityFrameworkCore.Migrations;

namespace Bubelsoft.Building.Infrastructure.Migrations
{
    public partial class CompanyinEstimation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Estimations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Estimations");
        }
    }
}
