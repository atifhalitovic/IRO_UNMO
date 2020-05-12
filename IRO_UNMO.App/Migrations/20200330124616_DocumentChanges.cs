using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class DocumentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameLearningAgreement",
                table: "Documents",
                newName: "TranscriptOfRecords");

            migrationBuilder.RenameColumn(
                name: "NameCV",
                table: "Documents",
                newName: "StudentTranscriptOfRecords");

            migrationBuilder.AddColumn<string>(
                name: "CertificateOfArrival",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateOfDeparture",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngProficiency",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivationLetter",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Passport",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceLetter",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentRecordSheet",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateOfArrival",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CertificateOfDeparture",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "EngProficiency",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MotivationLetter",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ReferenceLetter",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "StudentRecordSheet",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "TranscriptOfRecords",
                table: "Documents",
                newName: "NameLearningAgreement");

            migrationBuilder.RenameColumn(
                name: "StudentTranscriptOfRecords",
                table: "Documents",
                newName: "NameCV");
        }
    }
}
