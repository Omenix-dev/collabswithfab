using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class PatientReferrer : Base
	{
		public string ClinicId { get; set; }
		public string Notes { get; set; }
	}
}
