using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class ImmunizationDocument : Base
	{
		public string DocName { get; set; }
		public string DocPath { get; set; }




		//Navigation Properties
		public int ImmunizationId { get; set; }
		public virtual Immunization Immunization { get; set; }
	}
}
