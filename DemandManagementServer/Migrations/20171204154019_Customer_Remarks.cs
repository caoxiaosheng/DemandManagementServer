using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemandManagementServer.Migrations
{
    public partial class Customer_Remarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Customers",
                nullable: true);
        }
    }
}
