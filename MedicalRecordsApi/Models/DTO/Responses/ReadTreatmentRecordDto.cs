using MedicalRecordsData.Entities.MedicalRecordsEntity;
using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadTreatmentRecordDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime DateOfVisit { get; set; }
        public double Temperature { get; set; }
        public string Age { get; set; }
        public double Weight { get; set; }
        public string Diagnosis { get; set; }
        public string AdditonalNote { get; set; }


        //Navigation Properties
        public int PatientId { get; set; }

        public int VisitId { get; set; }
    }
}
