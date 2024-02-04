using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class AssignedPatientsDTO
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string PatientId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string AssignedNurse { get; set; }
		public string Age { get; set; }
		public string DateCreated { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }
		public double Temperature { get; set; }
		public int Heart { get; set; }
		public string Resp { get; set; }
	}
}
