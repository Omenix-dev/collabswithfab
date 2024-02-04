
namespace MedicalRecordsRepository.DTO.ReferralDto
{
    public class GetPatientReferralDto
    {
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Diagnosis { get; set; }
        public string DateCreated { get; set; }
    }
}
