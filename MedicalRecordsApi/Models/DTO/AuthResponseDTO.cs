using MedicalRecordsApi.Utils;

namespace MedicalRecordsApi.Models.DTO
{
    public class AuthResponseDTO
    {
        public Jwt jwt { get; set; }
        public string RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public int? ClinicId { get; set; }
    }
}
