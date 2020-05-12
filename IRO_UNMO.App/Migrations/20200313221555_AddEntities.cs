using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRO_UNMO.App.Migrations
{
    public partial class AddEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    PlaceName = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeInstitution",
                columns: table => new
                {
                    HomeInstitutionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OfficialName = table.Column<string>(nullable: true),
                    DepartmentName = table.Column<string>(nullable: true),
                    LevelOfEducation = table.Column<string>(nullable: true),
                    CurrentTermOrYear = table.Column<string>(nullable: true),
                    StudyProgramme = table.Column<string>(nullable: true),
                    CoordinatorFullName = table.Column<string>(nullable: true),
                    CoordinatorPosition = table.Column<string>(nullable: true),
                    CoordinatorEmail = table.Column<string>(nullable: true),
                    CoordinatorAddress = table.Column<string>(nullable: true),
                    CoordinatorPhoneNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeInstitution", x => x.HomeInstitutionId);
                });

            migrationBuilder.CreateTable(
                name: "Info",
                columns: table => new
                {
                    InfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Gender = table.Column<string>(nullable: true),
                    NationalityId = table.Column<int>(nullable: false),
                    CitizenshipId = table.Column<int>(nullable: false),
                    SecondCitizenshipId = table.Column<int>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    PassportNumber = table.Column<string>(nullable: true),
                    PassportIssueDate = table.Column<string>(nullable: true),
                    PassportExpiryDate = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    MotherName = table.Column<string>(nullable: true),
                    MothersMaidenName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Info", x => x.InfoId);
                    table.ForeignKey(
                        name: "FK_Info_Country_CitizenshipId",
                        column: x => x.CitizenshipId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Info_Country_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Info_Country_SecondCitizenshipId",
                        column: x => x.SecondCitizenshipId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    LanguageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Native = table.Column<string>(nullable: true),
                    ForeignFirst = table.Column<string>(nullable: true),
                    ForeignFirstProficiency = table.Column<string>(nullable: true),
                    ForeignSecond = table.Column<string>(nullable: true),
                    ForeignSecondProficiency = table.Column<string>(nullable: true),
                    ForeignThird = table.Column<string>(nullable: true),
                    ForeignThirdProficiency = table.Column<string>(nullable: true),
                    ForeignExperienceNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    UniversityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.UniversityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CountryId",
                table: "Contact",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Info_CitizenshipId",
                table: "Info",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Info_NationalityId",
                table: "Info",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Info_SecondCitizenshipId",
                table: "Info",
                column: "SecondCitizenshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "HomeInstitution");

            migrationBuilder.DropTable(
                name: "Info");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "University");
        }
    }
}
