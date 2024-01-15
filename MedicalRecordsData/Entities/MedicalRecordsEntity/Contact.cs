using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Contact : Base
	{
		public string StateOfResidence { get; set; }
		public string LGAResidence { get; set; }
		public string City { get; set; }
		public string HomeAddress { get; set; }
		public string Phone { get; set; }
		public string AltPhone { get; set; }
		public string Email { get; set; }

		//Navigation Properties
		public int PatientId { get; set; }
	}
}
