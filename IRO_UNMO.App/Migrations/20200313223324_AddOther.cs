using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddOther : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtherId",
                table: "Application",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Other",
                columns: table => new
                {
                    OtherId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MedicalInfo = table.Column<string>(nullable: true),
                    AdditionalRequests = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Other", x => x.OtherId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Application_OtherId",
                table: "Application",
                column: "OtherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application",
                column: "OtherId",
                principalTable: "Other",
                principalColumn: "OtherId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Other_OtherId",
                table: "Application");

            migrationBuilder.DropTable(
                name: "Other");

            migrationBuilder.DropIndex(
                name: "IX_Application_OtherId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OtherId",
                table: "Application");
        }
    }
}
