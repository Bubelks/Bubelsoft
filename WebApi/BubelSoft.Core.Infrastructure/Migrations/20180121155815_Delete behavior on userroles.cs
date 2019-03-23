using Microsoft.EntityFrameworkCore.Migrations;

namespace BubelSoft.Core.Infrastructure.Migrations
{
    public partial class Deletebehavioronuserroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_BuildingCompany_BuildingId_CompanyId",
                table: "UserRole");

            migrationBuilder.AlterColumn<bool>(
                name: "IsReady",
                table: "Buildings",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_BuildingCompany_BuildingId_CompanyId",
                table: "UserRole",
                columns: new[] { "BuildingId", "CompanyId" },
                principalTable: "BuildingCompany",
                principalColumns: new[] { "BuildingId", "CompanyId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_BuildingCompany_BuildingId_CompanyId",
                table: "UserRole");

            migrationBuilder.AlterColumn<bool>(
                name: "IsReady",
                table: "Buildings",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_BuildingCompany_BuildingId_CompanyId",
                table: "UserRole",
                columns: new[] { "BuildingId", "CompanyId" },
                principalTable: "BuildingCompany",
                principalColumns: new[] { "BuildingId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
