using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BubelSoft.Building.Infrastructure.Migrations
{
    public partial class ReportIdInQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportQuantity",
                table: "ReportQuantity");

            migrationBuilder.DropIndex(
                name: "IX_ReportQuantity_ReportId",
                table: "ReportQuantity");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReportQuantity");

            migrationBuilder.AlterColumn<int>(
                name: "ReportId",
                table: "ReportQuantity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportQuantity",
                table: "ReportQuantity",
                columns: new[] { "ReportId", "EstimationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportQuantity",
                table: "ReportQuantity");

            migrationBuilder.AlterColumn<int>(
                name: "ReportId",
                table: "ReportQuantity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ReportQuantity",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportQuantity",
                table: "ReportQuantity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportQuantity_ReportId",
                table: "ReportQuantity",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
