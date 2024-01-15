using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Medication : Base
	{
		public string Name { get; set; }


		//Navigation Properties
		public int TreatmentId { get; set; }
		public virtual Treatment Treatment { get; set; }
	}
}
