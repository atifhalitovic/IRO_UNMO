using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class ApplicationAttributesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Contact_ContactId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_HomeInstitution_HomeInstitutionId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Info_InfoId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Language_LanguageId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application");

            migrationBuilder.AlterColumn<int>(
                name: "OtherId",
                table: "Application",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Application",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "InfoId",
                table: "Application",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "HomeInstitutionId",
                table: "Application",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Application",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "Application",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Contact_ContactId",
                table: "Application",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_HomeInstitution_HomeInstitutionId",
                table: "Application",
                column: "HomeInstitutionId",
                principalTable: "HomeInstitution",
                principalColumn: "HomeInstitutionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Info_InfoId",
                table: "Application",
                column: "InfoId",
                principalTable: "Info",
                principalColumn: "InfoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Language_LanguageId",
                table: "Application",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application",
                column: "OtherId",
                principalTable: "Other",
                principalColumn: "OtherId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Contact_ContactId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_HomeInstitution_HomeInstitutionId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Info_InfoId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Language_LanguageId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "Application");

            migrationBuilder.AlterColumn<int>(
                name: "OtherId",
                table: "Application",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Application",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InfoId",
                table: "Application",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HomeInstitutionId",
                table: "Application",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Application",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Contact_ContactId",
                table: "Application",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_HomeInstitution_HomeInstitutionId",
                table: "Application",
                column: "HomeInstitutionId",
                principalTable: "HomeInstitution",
                principalColumn: "HomeInstitutionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Info_InfoId",
                table: "Application",
                column: "InfoId",
                principalTable: "Info",
                principalColumn: "InfoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Language_LanguageId",
                table: "Application",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application",
                column: "OtherId",
                principalTable: "Other",
                principalColumn: "OtherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
