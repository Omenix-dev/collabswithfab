using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Immunization : Base
	{
		public Immunization()
		{
			ImmunizationDocuments = new HashSet<ImmunizationDocument>();
		}

		public string Vaccine { get; set; }
		public string VaccineBrand { get; set; }
		public string BatchId { get; set; }
		public double Quantity { get; set; }
		public int Age { get; set; }
		public double Weight { get; set; }
		public double Temperature { get; set; }
		public DateTime DateGiven { get; set; }
		public string Notes { get; set; }




		//Navigation Properties
		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }


		public virtual ICollection<ImmunizationDocument> ImmunizationDocuments { get; set; }
	}
}
