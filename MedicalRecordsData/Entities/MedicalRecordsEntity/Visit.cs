using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Visit : Base
	{
		public DateTime DateOfVisit { get; set; }
		public double Temperature { get; set; }
		public string BloodPressure { get; set; }
		public int HeartPulse { get; set; }
		public string Respiratory { get; set; }
		public int Height { get; set; }
		public int Weight { get; set; }
		public int DoctorId { get; set; }
		public int NurseId { get; set; }
		public string Notes { get; set; }


		//Navigation Properties
		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }
	}
}
