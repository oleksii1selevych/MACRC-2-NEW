using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AlteredUserAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_Issues_IssueId",
                table: "UserAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssignments_UserWorkShifts_UserWorkShiftId",
                table: "UserAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments");

            migrationBuilder.RenameTable(
                name: "UserAssignments",
                newName: "Assignments");

            migrationBuilder.RenameColumn(
                name: "UserAssignmentId",
                table: "Assignments",
                newName: "AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAssignments_UserWorkShiftId",
                table: "Assignments",
                newName: "IX_Assignments_UserWorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAssignments_IssueId",
                table: "Assignments",
                newName: "IX_Assignments_IssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Issues_IssueId",
                table: "Assignments",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_UserWorkShifts_UserWorkShiftId",
                table: "Assignments",
                column: "UserWorkShiftId",
                principalTable: "UserWorkShifts",
                principalColumn: "UserWorkShiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Issues_IssueId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_UserWorkShifts_UserWorkShiftId",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "UserAssignments");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "UserAssignments",
                newName: "UserAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_UserWorkShiftId",
                table: "UserAssignments",
                newName: "IX_UserAssignments_UserWorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_IssueId",
                table: "UserAssignments",
                newName: "IX_UserAssignments_IssueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAssignments",
                table: "UserAssignments",
                column: "UserAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_Issues_IssueId",
                table: "UserAssignments",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssignments_UserWorkShifts_UserWorkShiftId",
                table: "UserAssignments",
                column: "UserWorkShiftId",
                principalTable: "UserWorkShifts",
                principalColumn: "UserWorkShiftId");
        }
    }
}
