using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Treatment : Base
	{
		public string DateOfVisit { get; set; }
		public string Temperature { get; set; }
		public string Age { get; set; }
		public string NurseId { get; set; }
		public string Weight { get; set; }
		public string Diagnosis { get; set; }
		public List<Medication> Medication { get; set; }
	}
}
