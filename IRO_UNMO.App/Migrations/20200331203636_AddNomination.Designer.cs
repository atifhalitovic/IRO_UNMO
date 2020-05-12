﻿// <auto-generated />
using System;
using IRO_UNMO.App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IRO_UNMO.App.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200331203636_AddNomination")]
    partial class AddNomination
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IRO_UNMO.App.Models.Administrator", b =>
                {
                    b.Property<string>("AdministratorId");

                    b.HasKey("AdministratorId");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Applicant", b =>
                {
                    b.Property<string>("ApplicantId");

                    b.Property<DateTime>("CreatedProfile");

                    b.Property<string>("FacultyName");

                    b.Property<DateTime>("LastEdited");

                    b.Property<string>("StudyCycle");

                    b.Property<string>("StudyField");

                    b.Property<string>("TypeOfApplication");

                    b.Property<int>("UniversityId");

                    b.Property<bool>("Verified");

                    b.HasKey("ApplicantId");

                    b.HasIndex("UniversityId");

                    b.ToTable("Applicant");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplicantId");

                    b.Property<int?>("ContactId");

                    b.Property<DateTime>("CreatedApp");

                    b.Property<int?>("DocumentsId");

                    b.Property<int?>("HomeInstitutionId");

                    b.Property<int?>("InfoId");

                    b.Property<int?>("LanguageId");

                    b.Property<DateTime>("LastEdited");

                    b.Property<int?>("OtherId");

                    b.HasKey("ApplicationId");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("ContactId");

                    b.HasIndex("DocumentsId");

                    b.HasIndex("HomeInstitutionId");

                    b.HasIndex("InfoId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("OtherId");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int>("CountryId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UniqueCode");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId");

                    b.Property<string>("Email");

                    b.Property<string>("PlaceName");

                    b.Property<string>("PostalCode");

                    b.Property<string>("StreetName");

                    b.Property<string>("Telephone");

                    b.HasKey("ContactId");

                    b.HasIndex("CountryId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Documents", b =>
                {
                    b.Property<int>("DocumentsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CV");

                    b.Property<string>("CertificateOfArrival");

                    b.Property<string>("CertificateOfDeparture");

                    b.Property<string>("EngProficiency");

                    b.Property<string>("LearningAgreement");

                    b.Property<string>("MotivationLetter");

                    b.Property<string>("Passport");

                    b.Property<string>("ReferenceLetter");

                    b.Property<string>("StudentRecordSheet");

                    b.Property<string>("StudentTranscriptOfRecords");

                    b.Property<string>("TranscriptOfRecords");

                    b.HasKey("DocumentsId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.HomeInstitution", b =>
                {
                    b.Property<int>("HomeInstitutionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CoordinatorAddress");

                    b.Property<string>("CoordinatorEmail");

                    b.Property<string>("CoordinatorFullName");

                    b.Property<string>("CoordinatorPhoneNum");

                    b.Property<string>("CoordinatorPosition");

                    b.Property<string>("CurrentTermOrYear");

                    b.Property<string>("DepartmentName");

                    b.Property<string>("LevelOfEducation");

                    b.Property<string>("OfficialName");

                    b.Property<string>("StudyProgramme");

                    b.HasKey("HomeInstitutionId");

                    b.ToTable("HomeInstitution");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Info", b =>
                {
                    b.Property<int>("InfoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CitizenshipId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FatherName");

                    b.Property<string>("Gender");

                    b.Property<string>("MotherName");

                    b.Property<string>("MothersMaidenName");

                    b.Property<DateTime>("PassportExpiryDate");

                    b.Property<DateTime>("PassportIssueDate");

                    b.Property<string>("PassportNumber");

                    b.Property<string>("PlaceOfBirth");

                    b.Property<int?>("SecondCitizenshipId");

                    b.HasKey("InfoId");

                    b.HasIndex("CitizenshipId");

                    b.HasIndex("SecondCitizenshipId");

                    b.ToTable("Info");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Language", b =>
                {
                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ForeignExperienceNumber");

                    b.Property<string>("ForeignFirst");

                    b.Property<string>("ForeignFirstProficiency");

                    b.Property<string>("ForeignSecond");

                    b.Property<string>("ForeignSecondProficiency");

                    b.Property<string>("ForeignThird");

                    b.Property<string>("ForeignThirdProficiency");

                    b.Property<string>("Native");

                    b.HasKey("LanguageId");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Nomination", b =>
                {
                    b.Property<int>("NominationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CV");

                    b.Property<string>("EngProficiency");

                    b.Property<string>("LearningAgreement");

                    b.Property<string>("MotivationLetter");

                    b.Property<string>("ReferenceLetter");

                    b.Property<string>("StatusOfNomination");

                    b.Property<string>("TranscriptOfRecords");

                    b.Property<bool>("Verified");

                    b.HasKey("NominationId");

                    b.ToTable("Nomination");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Other", b =>
                {
                    b.Property<int>("OtherId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalRequests");

                    b.Property<string>("MedicalInfo");

                    b.HasKey("OtherId");

                    b.ToTable("Other");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.University", b =>
                {
                    b.Property<int>("UniversityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("UniversityId");

                    b.ToTable("University");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Administrator", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("AdministratorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Applicant", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IRO_UNMO.App.Models.University", "University")
                        .WithMany()
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Application", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.Applicant", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId");

                    b.HasOne("IRO_UNMO.App.Models.Contact", "Contacts")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("IRO_UNMO.App.Models.Documents", "Documents")
                        .WithMany()
                        .HasForeignKey("DocumentsId");

                    b.HasOne("IRO_UNMO.App.Models.HomeInstitution", "HomeInstitutions")
                        .WithMany()
                        .HasForeignKey("HomeInstitutionId");

                    b.HasOne("IRO_UNMO.App.Models.Info", "Infos")
                        .WithMany()
                        .HasForeignKey("InfoId");

                    b.HasOne("IRO_UNMO.App.Models.Language", "Languages")
                        .WithMany()
                        .HasForeignKey("LanguageId");

                    b.HasOne("IRO_UNMO.App.Models.Other", "Others")
                        .WithMany()
                        .HasForeignKey("OtherId");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.ApplicationUser", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.City", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Contact", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("IRO_UNMO.App.Models.Info", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.Country", "Citizenship")
                        .WithMany()
                        .HasForeignKey("CitizenshipId");

                    b.HasOne("IRO_UNMO.App.Models.Country", "SecondCitizenship")
                        .WithMany()
                        .HasForeignKey("SecondCitizenshipId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("IRO_UNMO.App.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
