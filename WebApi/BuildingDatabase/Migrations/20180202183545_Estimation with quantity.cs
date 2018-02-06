using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BuildingContext.Migrations
{
    public partial class Estimationwithquantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Estimations_EstimationId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportQuantity_Report_ReportId",
                table: "ReportQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EstimationId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EstimationId",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "EstimationId",
                table: "ReportQuantity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReportQuantity_EstimationId",
                table: "ReportQuantity",
                column: "EstimationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportQuantity_Estimations_EstimationId",
                table: "ReportQuantity",
                column: "EstimationId",
                principalTable: "Estimations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportQuantity_Estimations_EstimationId",
                table: "ReportQuantity");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportQuantity_Reports_ReportId",
                table: "ReportQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_ReportQuantity_EstimationId",
                table: "ReportQuantity");

            migrationBuilder.DropColumn(
                name: "EstimationId",
                table: "ReportQuantity");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.AddColumn<int>(
                name: "EstimationId",
                table: "Report",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EstimationId",
                table: "Report",
                column: "EstimationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Estimations_EstimationId",
                table: "Report",
                column: "EstimationId",
                principalTable: "Estimations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportQuantity_Report_ReportId",
                table: "ReportQuantity",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
