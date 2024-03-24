
using MedicalRecordsData.Enum;

namespace MedicalRecordsRepository.DTO.ReferralDto
{
    public class GetPatientReferralDto
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public string HospitalName { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Diagnosis { get; set; }
        public string DateCreated { get; set; }
        public string AcceptanceStatus { get; set; }
    }
}
