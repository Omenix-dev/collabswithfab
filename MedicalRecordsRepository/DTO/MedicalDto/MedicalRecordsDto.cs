using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.MedicalDto
{
    public class MedicalRecordsDto
    {
        [Required(ErrorMessage = "Medical Record type is required")]
        public MedicalRecordType? MedicalRecordType { get; set; } //Allergy etc
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        [Required (ErrorMessage = "Patient Id is required")]
        public int? PatientId { get; set; }
        [Required (ErrorMessage ="Record Id is required")]
        public int? RecordId { get; set; }
    }
}
