﻿using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadCustomerFeedbackDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string Comments { get; set; }
	}
}
