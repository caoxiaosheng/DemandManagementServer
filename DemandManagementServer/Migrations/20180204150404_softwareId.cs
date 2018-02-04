using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemandManagementServer.Migrations
{
    public partial class softwareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demands_SoftwareVersions_SoftwareVersionId",
                table: "Demands");

            migrationBuilder.AlterColumn<int>(
                name: "SoftwareVersionId",
                table: "Demands",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_SoftwareVersions_SoftwareVersionId",
                table: "Demands",
                column: "SoftwareVersionId",
                principalTable: "SoftwareVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demands_SoftwareVersions_SoftwareVersionId",
                table: "Demands");

            migrationBuilder.AlterColumn<int>(
                name: "SoftwareVersionId",
                table: "Demands",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Demands_SoftwareVersions_SoftwareVersionId",
                table: "Demands",
                column: "SoftwareVersionId",
                principalTable: "SoftwareVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
