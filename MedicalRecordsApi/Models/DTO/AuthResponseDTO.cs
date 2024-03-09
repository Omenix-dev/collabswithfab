using MedicalRecordsApi.Utils;

namespace MedicalRecordsApi.Models.DTO
{
    public class AuthResponseDTO
    {
        public Jwt jwt { get; set; }
        public string RefreshToken { get; set; }

    }
}
