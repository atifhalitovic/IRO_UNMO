using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class PassportDateTimeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_Country_NationalityId",
                table: "Info");

            migrationBuilder.DropIndex(
                name: "IX_Info_NationalityId",
                table: "Info");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "Info");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportIssueDate",
                table: "Info",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportExpiryDate",
                table: "Info",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PassportIssueDate",
                table: "Info",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "PassportExpiryDate",
                table: "Info",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "Info",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Info_NationalityId",
                table: "Info",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Info_Country_NationalityId",
                table: "Info",
                column: "NationalityId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
