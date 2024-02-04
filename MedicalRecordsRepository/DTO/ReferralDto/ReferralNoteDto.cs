

namespace MedicalRecordsRepository.DTO.ReferralDto
{
    public class ReferralNoteDto
    {
        public int ReferralNoteId { get; set; }
        public int ClinicId { get; set; }
        public string Notes { get; set; }
        public int PatientId { get; set; }
    }
}
