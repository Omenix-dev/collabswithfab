using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public class NurseNote : Base
	{
		public string Note { get; set; }


		//Navigation Properties
		public int VisitId { get; set; }
		public virtual Visit Visit { get; set; }
	}
}
