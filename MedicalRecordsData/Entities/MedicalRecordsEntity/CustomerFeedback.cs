using MedicalRecordsData.Entities.BaseEntity;
using MedicalRecordsData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public class CustomerFeedback : Base
	{
		public ReviewRating Rating { get; set; }
		public string Comments { get; set; }
		public ReviewSource Source { get; set; }

		//Navigation Properties
		public int EmployeeId { get; set; }

	}
}
