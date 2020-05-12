using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddDocuments3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentsId",
                table: "Application",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Application_DocumentsId",
                table: "Application",
                column: "DocumentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Documents_DocumentsId",
                table: "Application",
                column: "DocumentsId",
                principalTable: "Documents",
                principalColumn: "DocumentsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Documents_DocumentsId",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_DocumentsId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "DocumentsId",
                table: "Application");
        }
    }
}
