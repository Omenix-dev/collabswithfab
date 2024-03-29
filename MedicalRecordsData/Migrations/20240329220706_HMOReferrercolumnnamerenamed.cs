using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class HMOReferrercolumnnamerenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HMOId",
                table: "PatientHmo");

            migrationBuilder.AddColumn<int>(
                name: "HMOProviderId",
                table: "PatientHmo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HMOProviderId",
                table: "PatientHmo");

            migrationBuilder.AddColumn<int>(
                name: "HMOId",
                table: "PatientHmo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
