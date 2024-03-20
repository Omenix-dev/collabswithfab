using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalRecordsData.Migrations
{
    public partial class updatedtheresidencetypoerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateOfResidnece",
                table: "EmergencyContacts");

            migrationBuilder.AddColumn<string>(
                name: "StateOfResidence",
                table: "EmergencyContacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateOfResidence",
                table: "EmergencyContacts");

            migrationBuilder.AddColumn<string>(
                name: "StateOfResidnece",
                table: "EmergencyContacts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
