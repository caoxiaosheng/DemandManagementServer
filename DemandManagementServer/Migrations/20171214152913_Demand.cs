using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemandManagementServer.Migrations
{
    public partial class Demand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Demands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DemandCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandPhase = table.Column<int>(type: "int", nullable: false),
                    DemandType = table.Column<int>(type: "int", nullable: false),
                    SoftwareVersionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demands_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demands_SoftwareVersions_SoftwareVersionId",
                        column: x => x.SoftwareVersionId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Demands_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DemandId = table.Column<int>(type: "int", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    RecordDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationRecords_Demands_DemandId",
                        column: x => x.DemandId,
                        principalTable: "Demands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demands_CustomerId",
                table: "Demands",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_SoftwareVersionId",
                table: "Demands",
                column: "SoftwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Demands_UserId",
                table: "Demands",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationRecords_DemandId",
                table: "OperationRecords",
                column: "DemandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationRecords");

            migrationBuilder.DropTable(
                name: "Demands");
        }
    }
}
