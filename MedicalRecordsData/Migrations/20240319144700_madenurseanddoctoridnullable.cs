using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class madenurseanddoctoridnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CustomerFeedbacks_Employees_EmployeeId",
            //    table: "CustomerFeedbacks");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Employees_Roles_RoleId",
            //    table: "Employees");

            //migrationBuilder.DropTable(
            //    name: "Clinics");

            //migrationBuilder.DropTable(
            //    name: "Departments");

            //migrationBuilder.DropTable(
            //    name: "UserRoles");

            //migrationBuilder.DropIndex(
            //    name: "IX_Employees_RoleId",
            //    table: "Employees");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomerFeedbacks_EmployeeId",
            //    table: "CustomerFeedbacks");

            //migrationBuilder.DropColumn(
            //    name: "DeletedAt",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "FirstName",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "Gender",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "IsDeleted",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "LastName",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "Password",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "Picture",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "Role",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "ActionTaken",
            //    table: "Roles");

            //migrationBuilder.RenameColumn(
            //    name: "isSettings",
            //    table: "Users",
            //    newName: "IsSettings");

            migrationBuilder.RenameColumn(
                name: "LGA",
                table: "Patients",
                newName: "Lga");

            migrationBuilder.RenameColumn(
                name: "HasHMO",
                table: "Patients",
                newName: "HasHmo");

            migrationBuilder.RenameColumn(
                name: "LGA",
                table: "EmergencyContacts",
                newName: "Lga");

            migrationBuilder.RenameColumn(
                name: "LGAResidence",
                table: "Contacts",
                newName: "LgaResidence");

            //migrationBuilder.AddColumn<int>(
            //    name: "RoleId",
            //    table: "Users",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "StaffId",
            //    table: "Users",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "ModifiedAt",
            //    table: "Roles",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2",
            //    oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddColumn<string>(
            //    name: "AccountStatus",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Department",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Designation",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsSuperAdmin",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "OnboardingDate",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "UserId",
            //    table: "Employees",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "Username",
            //    table: "Employees",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "EmployeePrivilegeAccesses",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        Status = table.Column<int>(nullable: false),
            //        EmployeeId = table.Column<int>(nullable: false),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: false),
            //        CreatedAt = table.Column<DateTime>(nullable: false),
            //        ModifiedAt = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmployeePrivilegeAccesses", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_EmployeePrivilegeAccesses_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RolePrivilegeAccess",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        Status = table.Column<int>(nullable: false),
            //        RoleId = table.Column<int>(nullable: false),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: false),
            //        CreatedAt = table.Column<DateTime>(nullable: false),
            //        ModifiedAt = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RolePrivilegeAccess", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_RolePrivilegeAccess_Roles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "Roles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientAssignmentHistories_PatientId",
            //    table: "PatientAssignmentHistories",
            //    column: "PatientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmployeePrivilegeAccesses_EmployeeId",
            //    table: "EmployeePrivilegeAccesses",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RolePrivilegeAccess_RoleId",
            //    table: "RolePrivilegeAccess",
            //    column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAssignmentHistories_Patients_PatientId",
                table: "PatientAssignmentHistories",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAssignmentHistories_Patients_PatientId",
                table: "PatientAssignmentHistories");

            migrationBuilder.DropTable(
                name: "EmployeePrivilegeAccesses");

            migrationBuilder.DropTable(
                name: "RolePrivilegeAccess");

            migrationBuilder.DropIndex(
                name: "IX_PatientAssignmentHistories_PatientId",
                table: "PatientAssignmentHistories");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsSuperAdmin",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OnboardingDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "IsSettings",
                table: "Users",
                newName: "isSettings");

            migrationBuilder.RenameColumn(
                name: "Lga",
                table: "Patients",
                newName: "LGA");

            migrationBuilder.RenameColumn(
                name: "HasHmo",
                table: "Patients",
                newName: "HasHMO");

            migrationBuilder.RenameColumn(
                name: "Lga",
                table: "EmergencyContacts",
                newName: "LGA");

            migrationBuilder.RenameColumn(
                name: "LgaResidence",
                table: "Contacts",
                newName: "LGAResidence");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "Patients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Patients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateEstablished = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mandate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateEstablished = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mandate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_CustomerFeedbacks_EmployeeId",
                table: "CustomerFeedbacks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_EmployeeId",
                table: "UserRoles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_Employees_EmployeeId",
                table: "CustomerFeedbacks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
