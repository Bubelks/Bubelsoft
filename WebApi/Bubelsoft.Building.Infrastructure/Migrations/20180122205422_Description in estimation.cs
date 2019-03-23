using Microsoft.EntityFrameworkCore.Migrations;

namespace Bubelsoft.Building.Infrastructure.Migrations
{
    public partial class Descriptioninestimation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Estimations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Estimations");
        }
    }
}
