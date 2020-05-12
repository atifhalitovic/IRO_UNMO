using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class CounryUpdte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThreeCharCC",
                table: "Country",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoCharCC",
                table: "Country",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreeCharCC",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "TwoCharCC",
                table: "Country");
        }
    }
}
