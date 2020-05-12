using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedProfile",
                table: "Applicant",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FacultyName",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudyCycle",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudyField",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfApplication",
                table: "Applicant",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "Applicant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_UniversityId",
                table: "Applicant",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicant_University_UniversityId",
                table: "Applicant",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicant_University_UniversityId",
                table: "Applicant");

            migrationBuilder.DropIndex(
                name: "IX_Applicant_UniversityId",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "CreatedProfile",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "FacultyName",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "StudyCycle",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "StudyField",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "TypeOfApplication",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Applicant");
        }
    }
}
