using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemandManagementServer.Migrations
{
    public partial class addSoftwareVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwareVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpectedEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VersionName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VersionProgress = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_VersionName",
                table: "SoftwareVersions",
                column: "VersionName",
                unique: true,
                filter: "[VersionName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareVersions");
        }
    }
}
