using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AppTimingConnect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimingId",
                table: "Application",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Application_TimingId",
                table: "Application",
                column: "TimingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Timing_TimingId",
                table: "Application",
                column: "TimingId",
                principalTable: "Timing",
                principalColumn: "TimingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Timing_TimingId",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_TimingId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "TimingId",
                table: "Application");
        }
    }
}
