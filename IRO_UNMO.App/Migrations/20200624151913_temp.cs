using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class temp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Offer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_UniversityId",
                table: "Offer",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_University_UniversityId",
                table: "Offer",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_University_UniversityId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_UniversityId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Offer");
        }
    }
}
