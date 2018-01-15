using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApi.Migrations
{
    public partial class RoletoUserBuildingRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserRole");

            migrationBuilder.AddColumn<int>(
                name: "UserBuildingRole",
                table: "UserRole",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserBuildingRole",
                table: "UserRole");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserRole",
                nullable: false,
                defaultValue: 0);
        }
    }
}
