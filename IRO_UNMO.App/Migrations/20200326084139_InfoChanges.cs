using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class InfoChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_Country_CitizenshipId",
                table: "Info");

            migrationBuilder.AlterColumn<int>(
                name: "CitizenshipId",
                table: "Info",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Info_Country_CitizenshipId",
                table: "Info",
                column: "CitizenshipId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_Country_CitizenshipId",
                table: "Info");

            migrationBuilder.AlterColumn<int>(
                name: "CitizenshipId",
                table: "Info",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Info_Country_CitizenshipId",
                table: "Info",
                column: "CitizenshipId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
