
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class UpdateEmergencyContactDto
    {
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        public string ContactAddress { get; set; }
        public string StateOfResidence { get; set; }
        public string Lga { get; set; }
        public string City { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number format")]
        public string AltPhone { get; set; }
        public int PatientId { get; set; }

    }
}
