using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class UpdatesUserWorkshiftsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_UserWorkShifts_UserWorkShiftId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkShifts_Users_UserId",
                table: "UserWorkShifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkShifts",
                table: "UserWorkShifts");

            migrationBuilder.RenameTable(
                name: "UserWorkShifts",
                newName: "WorkShifts");

            migrationBuilder.RenameColumn(
                name: "UserWorkShiftId",
                table: "Assignments",
                newName: "WorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_UserWorkShiftId",
                table: "Assignments",
                newName: "IX_Assignments_WorkShiftId");

            migrationBuilder.RenameColumn(
                name: "UserWorkShiftId",
                table: "WorkShifts",
                newName: "WorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWorkShifts_UserId",
                table: "WorkShifts",
                newName: "IX_WorkShifts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "WorkShifts",
                newName: "IX_WorkShifts_SmartBraceletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkShifts",
                table: "WorkShifts",
                column: "WorkShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_WorkShifts_WorkShiftId",
                table: "Assignments",
                column: "WorkShiftId",
                principalTable: "WorkShifts",
                principalColumn: "WorkShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShifts_SmartBracelets_SmartBraceletId",
                table: "WorkShifts",
                column: "SmartBraceletId",
                principalTable: "SmartBracelets",
                principalColumn: "SmartBraceletId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkShifts_Users_UserId",
                table: "WorkShifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_WorkShifts_WorkShiftId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkShifts_SmartBracelets_SmartBraceletId",
                table: "WorkShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkShifts_Users_UserId",
                table: "WorkShifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkShifts",
                table: "WorkShifts");

            migrationBuilder.RenameTable(
                name: "WorkShifts",
                newName: "UserWorkShifts");

            migrationBuilder.RenameColumn(
                name: "WorkShiftId",
                table: "Assignments",
                newName: "UserWorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_WorkShiftId",
                table: "Assignments",
                newName: "IX_Assignments_UserWorkShiftId");

            migrationBuilder.RenameColumn(
                name: "WorkShiftId",
                table: "UserWorkShifts",
                newName: "UserWorkShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkShifts_UserId",
                table: "UserWorkShifts",
                newName: "IX_UserWorkShifts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkShifts_SmartBraceletId",
                table: "UserWorkShifts",
                newName: "IX_UserWorkShifts_SmartBraceletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkShifts",
                table: "UserWorkShifts",
                column: "UserWorkShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_UserWorkShifts_UserWorkShiftId",
                table: "Assignments",
                column: "UserWorkShiftId",
                principalTable: "UserWorkShifts",
                principalColumn: "UserWorkShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId",
                principalTable: "SmartBracelets",
                principalColumn: "SmartBraceletId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkShifts_Users_UserId",
                table: "UserWorkShifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
