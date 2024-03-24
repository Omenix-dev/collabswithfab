
using System.ComponentModel.DataAnnotations;
using System;

namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class UpdateContactDto
    {
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }
        public string HomeAddress { get; set; }
        public string StateOfResidence { get; set; }
        public string LgaResidence { get; set; }
        public string City { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number format")]
        public string AltPhone { get; set; }
        public int PatientId { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
    }
}
