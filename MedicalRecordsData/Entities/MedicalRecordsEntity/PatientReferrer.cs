using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class PatientReferrer : Base
	{
		public int ClinicId { get; set; }
		public string Notes { get; set; }


		//Navigation Properties
		public int PatientId { get; set; }
	}
}
