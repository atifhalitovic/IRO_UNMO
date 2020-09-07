using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class RemoveLastEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "Applicant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "Applicant",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
