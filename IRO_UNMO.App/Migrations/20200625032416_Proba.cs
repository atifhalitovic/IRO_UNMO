using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class Proba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination");

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "Nomination",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Nomination",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Nomination_OfferId",
                table: "Nomination",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_Offer_OfferId",
                table: "Nomination",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "OfferId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_Offer_OfferId",
                table: "Nomination");

            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination");

            migrationBuilder.DropIndex(
                name: "IX_Nomination_OfferId",
                table: "Nomination");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Nomination");

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "Nomination",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
