using MedicalRecordsData.Entities.BaseEntity;
using MedicalRecordsData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class PatientReferrer : Base
	{
		public int ReferredClinicId { get; set; }
		public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public string Notes { get; set; }
		public AcceptanceStatus AcceptanceStatus { get; set; }
		//Navigation Properties
		public int PatientId { get; set; }
		public Patient Patient { get; set; }
		public int TreatmentId { get; set; }
		public Treatment Treatment { get; set; }
	}
}
