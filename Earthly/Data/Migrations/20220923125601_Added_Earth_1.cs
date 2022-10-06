using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Earthly.Data.Migrations
{
    public partial class Added_Earth_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT Earth ON");
            migrationBuilder.Sql("INSERT INTO Earth(Id, NumberOfCountries) VALUES (1,0)");
            migrationBuilder.Sql("SET IDENTITY_INSERT Earth OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Earth WHERE Id = 1");
        }
    }
}
