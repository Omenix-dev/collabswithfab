using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class madethevisitrecosrtdoctorandnursenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BedAssignments");

            //migrationBuilder.DropTable(
            //    name: "Facilities");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "Visits",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Visits",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.CreateTable(
            //    name: "AssignPatientBeds",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BedId = table.Column<int>(nullable: false),
            //        PatientAssignedName = table.Column<string>(nullable: true),
            //        AssignerUserId = table.Column<int>(nullable: false),
            //        PatientAssignedId = table.Column<int>(nullable: false),
            //        PatientAssignedUserId = table.Column<int>(nullable: false),
            //        AssignNote = table.Column<string>(nullable: true),
            //        UnAssignerUserId = table.Column<int>(nullable: false),
            //        BedAssignDate = table.Column<DateTime>(nullable: false),
            //        BedUnAssignDate = table.Column<DateTime>(nullable: false),
            //        Status = table.Column<int>(nullable: false),
            //        ActionTaken = table.Column<string>(nullable: true),
            //        AssignedBy = table.Column<int>(nullable: false),
            //        UnAssignedBy = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AssignPatientBeds", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Beds",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        Location = table.Column<string>(nullable: true),
            //        Note = table.Column<string>(nullable: true),
            //        UserId = table.Column<int>(nullable: false),
            //        Status = table.Column<string>(nullable: true),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: false),
            //        CreatedAt = table.Column<DateTime>(nullable: false),
            //        UpdatedAt = table.Column<DateTime>(nullable: false),
            //        ActionTaken = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Beds", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignPatientBeds");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "Visits",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Visits",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BedAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacilityType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });
        }
    }
}
