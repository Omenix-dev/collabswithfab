using System;
using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Request
{
	public class CreatePatientPrescriptionDTO
	{
		public DateTime DateOfVisit { get; set; }
		public string Diagnosis { get; set; }
		public List<string> Medication { get; set; }
	}
}
