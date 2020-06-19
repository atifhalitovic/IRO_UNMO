using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class NominationFinishedCheckboxDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedTime",
                table: "Nomination",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedTime",
                table: "Nomination");
        }
    }
}
