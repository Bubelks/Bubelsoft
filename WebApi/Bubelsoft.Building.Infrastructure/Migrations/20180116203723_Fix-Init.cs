using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bubelsoft.Building.Infrastructure.Migrations
{
    public partial class FixInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Companies_EstimationId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Report_ReportId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_ReportId",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Estimations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estimations",
                table: "Estimations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReportQuantity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<decimal>(nullable: false),
                    ReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportQuantity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportQuantity_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportQuantity_ReportId",
                table: "ReportQuantity",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Estimations_EstimationId",
                table: "Report",
                column: "EstimationId",
                principalTable: "Estimations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Estimations_EstimationId",
                table: "Report");

            migrationBuilder.DropTable(
                name: "ReportQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estimations",
                table: "Estimations");

            migrationBuilder.RenameTable(
                name: "Estimations",
                newName: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "Report",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReportId",
                table: "Report",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Companies_EstimationId",
                table: "Report",
                column: "EstimationId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Report_ReportId",
                table: "Report",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
