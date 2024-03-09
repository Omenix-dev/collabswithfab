using System;
using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Request
{
	public class CreatePatientPrescriptionDto
	{
		public DateTime DateOfVisit { get; set; }
		public string Diagnosis { get; set; }
		public List<string> Medication { get; set; }
	}
}
