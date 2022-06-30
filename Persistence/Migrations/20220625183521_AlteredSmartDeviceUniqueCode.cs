using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AlteredSmartDeviceUniqueCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmartBracelets_ManufacturerCode",
                table: "SmartBracelets",
                column: "ManufacturerCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmartBracelets_ManufacturerCode",
                table: "SmartBracelets");
        }
    }
}
