using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class updateToPatientTableWithPatientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Visits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Treatments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Treatments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Treatments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Treatments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Patients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "PatientReferrers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Medications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Medications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Medications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Medications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "MedicalRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MedicalRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "MedicalRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MedicalRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Immunizations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Immunizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Immunizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Immunizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "ImmunizationDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ImmunizationDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "ImmunizationDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ImmunizationDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "EmergencyContacts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EmergencyContacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "EmergencyContacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EmergencyContacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Contacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    DateEstablished = table.Column<string>(nullable: true),
                    Mandate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    DateEstablished = table.Column<string>(nullable: true),
                    Mandate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiEndPoint = table.Column<string>(nullable: true),
                    HomeLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    SalaryAccountNumber = table.Column<string>(nullable: true),
                    SalaryDomiciledBank = table.Column<string>(nullable: true),
                    Nin = table.Column<string>(nullable: true),
                    Bvn = table.Column<string>(nullable: true),
                    StaffId = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    MaritalStatus = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    AuthenticationToken = table.Column<string>(nullable: true),
                    NationalityId = table.Column<int>(nullable: false),
                    ReligionId = table.Column<int>(nullable: false),
                    ClinicId = table.Column<int>(nullable: true),
                    MotherMaidenName = table.Column<string>(nullable: true),
                    WeddingAnniversary = table.Column<string>(nullable: true),
                    ProfilePicture = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    NIN = table.Column<string>(nullable: true),
                    BVN = table.Column<string>(nullable: true),
                    WorkGrade = table.Column<string>(nullable: true),
                    ResumptionDate = table.Column<string>(nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    Signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    EmployeeId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_EmployeeId",
                table: "UserRoles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_EmployeeId1",
                table: "UserRoles",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "ImmunizationDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ImmunizationDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ImmunizationDocuments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ImmunizationDocuments");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
