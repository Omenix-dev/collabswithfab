using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class UpdateToFeedbackTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lab_Visits_VisitId",
                table: "Lab");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Employees_EmployeeId1",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_EmployeeId1",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lab",
                table: "Lab");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Lab",
                newName: "Labs");

            migrationBuilder.RenameIndex(
                name: "IX_Lab_VisitId",
                table: "Labs",
                newName: "IX_Labs_VisitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Labs",
                table: "Labs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BedAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: false),
                    FacilityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Source = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFeedbacks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    ActionTaken = table.Column<string>(nullable: true),
                    FacilityType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsOccupied = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_EmployeeId",
                table: "CustomerFeedbacks",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labs_Visits_VisitId",
                table: "Labs",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labs_Visits_VisitId",
                table: "Labs");

            migrationBuilder.DropTable(
                name: "BedAssignments");

            migrationBuilder.DropTable(
                name: "CustomerFeedbacks");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Labs",
                table: "Labs");

            migrationBuilder.RenameTable(
                name: "Labs",
                newName: "Lab");

            migrationBuilder.RenameIndex(
                name: "IX_Labs_VisitId",
                table: "Lab",
                newName: "IX_Lab_VisitId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lab",
                table: "Lab",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_EmployeeId1",
                table: "UserRoles",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Lab_Visits_VisitId",
                table: "Lab",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Employees_EmployeeId1",
                table: "UserRoles",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
