using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class anotherTryToSuccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Lattitude = table.Column<double>(type: "float", nullable: true),
                    Longtitude = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_Issues_Accidents_AccidentId",
                        column: x => x.AccidentId,
                        principalTable: "Accidents",
                        principalColumn: "AccidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignments",
                columns: table => new
                {
                    UserAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    UserWorkShiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignments", x => x.UserAssignmentId);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "IssueId");
                    table.ForeignKey(
                        name: "FK_UserAssignments_UserWorkShifts_UserWorkShiftId",
                        column: x => x.UserWorkShiftId,
                        principalTable: "UserWorkShifts",
                        principalColumn: "UserWorkShiftId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AccidentId",
                table: "Issues",
                column: "AccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_IssueId",
                table: "UserAssignments",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_UserWorkShiftId",
                table: "UserAssignments",
                column: "UserWorkShiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAssignments");

            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
