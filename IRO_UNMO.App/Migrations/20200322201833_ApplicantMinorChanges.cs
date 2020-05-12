using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class ApplicantMinorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastChange",
                table: "Application",
                newName: "LastEdited");

            migrationBuilder.RenameColumn(
                name: "CreatedProfile",
                table: "Application",
                newName: "CreatedApp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastEdited",
                table: "Application",
                newName: "LastChange");

            migrationBuilder.RenameColumn(
                name: "CreatedApp",
                table: "Application",
                newName: "CreatedProfile");
        }
    }
}
