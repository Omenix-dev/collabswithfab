using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadVisitHistoryDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime DateOfVisit { get; set; }
		public double Temperature { get; set; }
		public string BloodPressure { get; set; }
		public int HeartPulse { get; set; }
		public string Respiratory { get; set; }
		public string Age { get; set; }
		public int Height { get; set; }
		public int Weight { get; set; }
		public int DoctorId { get; set; }
		public int NurseId { get; set; }
		public string DoctorName { get; set; }
		public string NurseName { get; set; }


        //Navigation Properties
        public int PatientId { get; set; }
        public int TreatmentId { get; set; }
    }
}
