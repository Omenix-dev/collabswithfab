

using MedicalRecordsData.Enum;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.ReferralDto
{
    public class ReferralNoteDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The referred clinic id is required")]
        public int ReferralId { get; set; }
        public AcceptanceStatus AcceptanceStatus { get; set; }
        [Required(ErrorMessage = "the Note is required")]
        public string Notes { get; set; }
    }
    public class ReferralDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The clinic id is required")]
        public int ClinicId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The referred clinic id is required")]
        public int ReferredClinicId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Patient id is required")]
        public int PatientId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Treatement id is required")]
        public int TreatmentId { get; set; }
        [Required(ErrorMessage = "the Note is required")]
        public string ReferralNotes { get; set; }

    }
}
