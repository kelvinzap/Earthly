using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Earthly.Data.Migrations
{
    public partial class Added_CountryCode_And_ISO3_Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISO3",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ISO3",
                table: "Countries");
        }
    }
}
