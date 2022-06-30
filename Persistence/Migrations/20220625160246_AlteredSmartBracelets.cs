using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marc2.Migrations
{
    public partial class AlteredSmartBracelets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Spo2Percentage",
                table: "SmartBracelets",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PulseRate",
                table: "SmartBracelets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CertificatePassword",
                table: "SmartBracelets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificatePassword",
                table: "SmartBracelets");

            migrationBuilder.AlterColumn<double>(
                name: "Spo2Percentage",
                table: "SmartBracelets",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PulseRate",
                table: "SmartBracelets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
