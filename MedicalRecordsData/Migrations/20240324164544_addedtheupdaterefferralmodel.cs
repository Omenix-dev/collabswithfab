using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class addedtheupdaterefferralmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PatientReferrers_PatientId",
                table: "PatientReferrers");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentStatus",
                table: "Treatments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsReferred",
                table: "Patients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AcceptanceStatus",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReferredClinicId",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "PatientReferrers",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Clinics",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreatedAt = table.Column<DateTime>(nullable: false),
            //        ModifiedAt = table.Column<DateTime>(nullable: true),
            //        Status = table.Column<int>(nullable: false),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: false),
            //        ActionTaken = table.Column<string>(nullable: true),
            //        Name = table.Column<string>(nullable: true),
            //        Location = table.Column<string>(nullable: true),
            //        DateEstablished = table.Column<string>(nullable: true),
            //        Mandate = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Clinics", x => x.Id);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferrers_ClinicId",
                table: "PatientReferrers",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferrers_PatientId",
                table: "PatientReferrers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferrers_TreatmentId",
                table: "PatientReferrers",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientReferrers_Clinics_ClinicId",
                table: "PatientReferrers",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientReferrers_Treatments_TreatmentId",
                table: "PatientReferrers",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientReferrers_Clinics_ClinicId",
                table: "PatientReferrers");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientReferrers_Treatments_TreatmentId",
                table: "PatientReferrers");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_PatientReferrers_ClinicId",
                table: "PatientReferrers");

            migrationBuilder.DropIndex(
                name: "IX_PatientReferrers_PatientId",
                table: "PatientReferrers");

            migrationBuilder.DropIndex(
                name: "IX_PatientReferrers_TreatmentId",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "TreatmentStatus",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "IsReferred",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AcceptanceStatus",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "ReferredClinicId",
                table: "PatientReferrers");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "PatientReferrers");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferrers_PatientId",
                table: "PatientReferrers",
                column: "PatientId",
                unique: true);
        }
    }
}
