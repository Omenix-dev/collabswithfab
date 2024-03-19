
namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class UpdateContactDto
    {
        public string Relationship { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string HomeAddress { get; set; }
        public string StateOfResidence { get; set; }
        public string LgaResidence { get; set; }
        public string City { get; set; }
        public string AltPhone { get; set; }
        public int PatientId { get; set; }
    }
}
