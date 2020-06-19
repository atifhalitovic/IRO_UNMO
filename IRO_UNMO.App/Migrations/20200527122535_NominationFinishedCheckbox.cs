using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class NominationFinishedCheckbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Nomination",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Nomination");
        }
    }
}
