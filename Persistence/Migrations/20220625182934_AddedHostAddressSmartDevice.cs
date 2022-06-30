using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AddedHostAddressSmartDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificatePath",
                table: "SmartBracelets");

            migrationBuilder.AddColumn<string>(
                name: "HostAddress",
                table: "SmartBracelets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostAddress",
                table: "SmartBracelets");

            migrationBuilder.AddColumn<string>(
                name: "CertificatePath",
                table: "SmartBracelets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
