
using System;

namespace MedicalRecordsRepository.DTO.MedicalDto
{
    public class ImmunizationDto
    {
        public string Vaccine { get; set; }
        public string VaccineBrand { get; set; }
        public string BatchId { get; set; }
        public double Quantity { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Temperature { get; set; }
        public DateTime DateGiven { get; set; }
        public string Notes { get; set; }
        //Navigation Properties
        public int PatientId { get; set; }
        public int ImmunizationId { get; set; }
    }
}
