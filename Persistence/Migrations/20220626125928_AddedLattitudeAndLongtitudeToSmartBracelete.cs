using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AddedLattitudeAndLongtitudeToSmartBracelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Lattitude",
                table: "SmartBracelets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Logngtitude",
                table: "SmartBracelets",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lattitude",
                table: "SmartBracelets");

            migrationBuilder.DropColumn(
                name: "Logngtitude",
                table: "SmartBracelets");
        }
    }
}
