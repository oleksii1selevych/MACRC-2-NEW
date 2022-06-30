using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AlteredWorkshift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkShifts_TaskAssignments_TaskAssignmentId",
                table: "UserWorkShifts");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkShifts_TaskAssignmentId",
                table: "UserWorkShifts");

            migrationBuilder.DropColumn(
                name: "SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropColumn(
                name: "TaskAssignmentId",
                table: "UserWorkShifts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SmartBraceletId",
                table: "UserWorkShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskAssignmentId",
                table: "UserWorkShifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    TaskAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccidentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Lattitude = table.Column<double>(type: "float", nullable: true),
                    Longtitude = table.Column<double>(type: "float", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.TaskAssignmentId);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Accidents_AccidentId",
                        column: x => x.AccidentId,
                        principalTable: "Accidents",
                        principalColumn: "AccidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkShifts_TaskAssignmentId",
                table: "UserWorkShifts",
                column: "TaskAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_AccidentId",
                table: "TaskAssignments",
                column: "AccidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId",
                principalTable: "SmartBracelets",
                principalColumn: "SmartBraceletId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkShifts_TaskAssignments_TaskAssignmentId",
                table: "UserWorkShifts",
                column: "TaskAssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "TaskAssignmentId");
        }
    }
}
