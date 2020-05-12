using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddNominationWorkPlanForStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkPlan",
                table: "Nomination",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nomination_UniversityId",
                table: "Nomination",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "UniversityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_University_UniversityId",
                table: "Nomination");

            migrationBuilder.DropIndex(
                name: "IX_Nomination_UniversityId",
                table: "Nomination");

            migrationBuilder.DropColumn(
                name: "WorkPlan",
                table: "Nomination");
        }
    }
}
