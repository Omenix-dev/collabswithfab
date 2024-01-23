using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class UpdateToMappingOfTreatmentVisit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Visits");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "Visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisitId",
                table: "Treatments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NurseNote",
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
                    Note = table.Column<string>(nullable: true),
                    VisitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseNote_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_VisitId",
                table: "Treatments",
                column: "VisitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NurseNote_VisitId",
                table: "NurseNote",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Visits_VisitId",
                table: "Treatments",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Visits_VisitId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "NurseNote");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_VisitId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Treatments");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Visits",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
