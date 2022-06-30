using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AddedSmartBraceletsForeignKeyAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SmartBraceletId",
                table: "UserWorkShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts",
                column: "SmartBraceletId",
                principalTable: "SmartBracelets",
                principalColumn: "SmartBraceletId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkShifts_SmartBracelets_SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkShifts_SmartBraceletId",
                table: "UserWorkShifts");

            migrationBuilder.DropColumn(
                name: "SmartBraceletId",
                table: "UserWorkShifts");
        }
    }
}
