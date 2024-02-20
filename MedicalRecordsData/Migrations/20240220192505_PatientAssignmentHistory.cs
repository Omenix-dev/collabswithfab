using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class PatientAssignmentHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Patients");

            migrationBuilder.AddColumn<bool>(
                name: "HasHMO",
                table: "Patients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PatientAssignmentHistories",
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
                    NurseId = table.Column<int>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false),
                    CareType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAssignmentHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAssignmentHistories");

            migrationBuilder.DropColumn(
                name: "HasHMO",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
