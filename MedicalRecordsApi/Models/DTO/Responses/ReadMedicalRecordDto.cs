using MedicalRecordsData.Enum;
using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadMedicalRecordDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public MedicalRecordType MedicalRecordType { get; set; } //Allergy etc
        public string Name { get; set; }
        public string Comment { get; set; }

        public int PatientId { get; set; }
    }
}
