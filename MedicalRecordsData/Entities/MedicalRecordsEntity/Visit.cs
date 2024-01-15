using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Visit : Base
	{
		public string DateOfVisit { get; set; }
		public string Temperature { get; set; }
		public string BloodPressure { get; set; }
		public string HeartPulse { get; set; }
		public string Respiratory { get; set; }
		public string Height { get; set; }
		public string Weight { get; set; }
		public string DoctorId { get; set; }
		public string Notes { get; set; }
	}
}
