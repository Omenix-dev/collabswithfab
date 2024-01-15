using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Medication : Base
	{
		public string TreatmentId { get; set; }
		public string Name { get; set; }
	}
}
