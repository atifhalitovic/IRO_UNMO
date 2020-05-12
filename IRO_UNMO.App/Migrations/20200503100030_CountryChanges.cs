using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class CountryChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreeCharCC",
                table: "Country");

            migrationBuilder.RenameColumn(
                name: "TwoCharCC",
                table: "Country",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Country",
                newName: "TwoCharCC");

            migrationBuilder.AddColumn<string>(
                name: "ThreeCharCC",
                table: "Country",
                nullable: true);
        }
    }
}
