using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class UserWorkShiftsWereAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserWorkShifts",
                columns: table => new
                {
                    WorkShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SmartBraceletId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkShifts", x => x.WorkShiftId);
                    table.ForeignKey(
                        name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                        column: x => x.SmartBraceletId,
                        principalTable: "SmartBracelets",
                        principalColumn: "SmartBraceletId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkShifts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkShifts_UserId",
                table: "UserWorkShifts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkShifts");
        }
    }
}
