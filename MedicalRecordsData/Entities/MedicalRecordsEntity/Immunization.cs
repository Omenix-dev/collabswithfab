using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Immunization : Base
	{
		public string Vaccine { get; set; }
		public string VaccineBrand { get; set; }
		public string BatchId { get; set; }
		public string Quantity { get; set; }
		public string Age { get; set; }
		public string Weight { get; set; }
		public string Temperature { get; set; }
		public string DateGiven { get; set; }
		public string Notes { get; set; }
		public List<ImmunizationDocument> ImmunizationDocuments { get; set; } = new List<ImmunizationDocument>();
	}
}
