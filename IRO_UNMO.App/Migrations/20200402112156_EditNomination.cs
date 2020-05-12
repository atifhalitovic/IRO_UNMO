using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class EditNomination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantId",
                table: "Nomination",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedNom",
                table: "Nomination",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEdited",
                table: "Nomination",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "Nomination");

            migrationBuilder.DropColumn(
                name: "CreatedNom",
                table: "Nomination");

            migrationBuilder.DropColumn(
                name: "LastEdited",
                table: "Nomination");
        }
    }
}
