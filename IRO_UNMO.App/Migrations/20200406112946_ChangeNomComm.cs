using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class ChangeNomComm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination");

            migrationBuilder.AlterColumn<int>(
                name: "CommentsId",
                table: "Nomination",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination",
                column: "CommentsId",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination");

            migrationBuilder.AlterColumn<int>(
                name: "CommentsId",
                table: "Nomination",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Nomination_Comment_CommentsId",
                table: "Nomination",
                column: "CommentsId",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
