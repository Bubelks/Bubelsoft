using Microsoft.EntityFrameworkCore.Migrations;

namespace BubelSoft.Core.Infrastructure.Migrations
{
    public partial class connString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "Buildings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Buildings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Buildings");
        }
    }
}
