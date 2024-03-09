using System;
using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadImmunizationRecordDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Vaccine { get; set; }
        public string VaccineBrand { get; set; }
        public string BatchId { get; set; }
        public double Quantity { get; set; }
        public string Age { get; set; }
        public double Weight { get; set; }
        public double Temperature { get; set; }
        public DateTime DateGiven { get; set; }
        public string Notes { get; set; }

        public List<ReadImmunizationDocumentDto> Documents { get; set;}


        public int PatientId { get; set; }
    }

    public class ReadImmunizationDocumentDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DocName { get; set; }
        public string DocPath { get; set; }

        public int ImmunizationId { get; set; }
    }
}
