using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class SmartBracelets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartBracelets",
                columns: table => new
                {
                    SmartBraceletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PulseRate = table.Column<int>(type: "int", nullable: false),
                    Spo2Percentage = table.Column<double>(type: "float", nullable: false),
                    CertificatePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastRequest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartBracelets", x => x.SmartBraceletId);
                    table.ForeignKey(
                        name: "FK_SmartBracelets_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartBracelets_OrganizationId",
                table: "SmartBracelets",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartBracelets");
        }
    }
}
