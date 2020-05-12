using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddNomination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nomination",
                columns: table => new
                {
                    NominationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearningAgreement = table.Column<string>(nullable: true),
                    CV = table.Column<string>(nullable: true),
                    EngProficiency = table.Column<string>(nullable: true),
                    TranscriptOfRecords = table.Column<string>(nullable: true),
                    MotivationLetter = table.Column<string>(nullable: true),
                    ReferenceLetter = table.Column<string>(nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    StatusOfNomination = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomination", x => x.NominationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nomination");
        }
    }
}
