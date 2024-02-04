using MedicalRecordsData.Entities.BaseEntity;
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

	public enum ReviewRating
	{
		Excellent,
		JustOk,
		Poor
	}

	public enum ReviewSource
	{
		Patient,
		Colleague
	}
}
