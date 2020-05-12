using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class Dodano : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicantId",
                table: "Nomination",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nomination_ApplicantId",
                table: "Nomination",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_Applicant_ApplicantId",
                table: "Nomination",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "ApplicantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_Applicant_ApplicantId",
                table: "Nomination");

            migrationBuilder.DropIndex(
                name: "IX_Nomination_ApplicantId",
                table: "Nomination");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicantId",
                table: "Nomination",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
