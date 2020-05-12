using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class UniCountry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_University_CountryId",
                table: "University",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_University_Country_CountryId",
                table: "University",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_University_Country_CountryId",
                table: "University");

            migrationBuilder.DropIndex(
                name: "IX_University_CountryId",
                table: "University");
        }
    }
}
