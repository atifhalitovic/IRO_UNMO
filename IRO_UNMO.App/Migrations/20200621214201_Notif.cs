using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class Notif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignalRToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    UserToID = table.Column<string>(nullable: true),
                    UserFromID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserFromID",
                        column: x => x.UserFromID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserToID",
                        column: x => x.UserToID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserFromID",
                table: "Notification",
                column: "UserFromID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserToID",
                table: "Notification",
                column: "UserToID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropColumn(
                name: "SignalRToken",
                table: "AspNetUsers");
        }
    }
}
