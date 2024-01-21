using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Treatment : Base
	{
		public Treatment()
		{
			Medications = new HashSet<Medication>();
		}

		public DateTime DateOfVisit { get; set; }
		public double Temperature { get; set; }
		public string Age { get; set; }
		public double Weight { get; set; }
		public string Diagnosis { get; set; }


		//Navigation Properties
		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }


		public virtual ICollection<Medication> Medications { get; set; }
	}
}
