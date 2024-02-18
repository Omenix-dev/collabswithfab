using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;

namespace MedicalRecordsRepository.DTO.MedicalDto
{
    public class MedicalRecordsDto
    {
        public MedicalRecordType MedicalRecordType { get; set; } //Allergy etc
        public string Name { get; set; }
        public string Comment { get; set; }
        public int PatientId { get; set; }
        public int RecordId { get; set; }
    }
}
