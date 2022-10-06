using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Earthly.Data.Migrations
{
    public partial class Added_GmtOffsetandTimeStamp_Properties_To_CountryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GMTOffset",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeStamp",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GMTOffset",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Countries");
        }
    }
}
