using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class ApplicationFinishedTiming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Application",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedTime",
                table: "Application",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "FinishedTime",
                table: "Application");
        }
    }
}
