using MedicalRecordsData.Entities.MedicalRecordsEntity;
using System;
using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class AssignedPatientsDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NurseId { get; set; }
        public string AssignedNurse { get; set; }
		public string Age { get; set; }
		public string DateCreated { get; set; }
		//public int Weight { get; set; }
		//public int Height { get; set; }
		//public double Temperature { get; set; }
		//public int Heart { get; set; }
		//public string Resp { get; set; }
        public List<Visit> Visits { get; set; }
    }
}
