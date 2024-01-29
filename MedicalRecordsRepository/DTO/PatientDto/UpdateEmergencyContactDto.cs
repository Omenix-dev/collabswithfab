
namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class UpdateEmergencyContactDto
    {
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactAddress { get; set; }
        public string StateOfResidnece { get; set; }
        public string LGA { get; set; }
        public string City { get; set; }
        public string AltPhone { get; set; }
        public int PatientId { get; set; }

    }
}
