using MedicalRecordsData.Entities.BaseEntity;
using MedicalRecordsData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class MedicalRecord : Base
	{
		public MedicalRecordType MedicalRecordType { get; set; } //Allergy etc
		public string Name { get; set; }
		public string Comment { get; set; }


		//Navigation Properties
		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }
	}
}
