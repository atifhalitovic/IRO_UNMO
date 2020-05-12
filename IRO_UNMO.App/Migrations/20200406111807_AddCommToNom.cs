using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddCommToNom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsId",
                table: "Nomination",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Nomination_CommentsId",
                table: "Nomination",
                column: "CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination",
                column: "CommentsId",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination");

            migrationBuilder.DropIndex(
                name: "IX_Nomination_CommentsId",
                table: "Nomination");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "Nomination");
        }
    }
}
