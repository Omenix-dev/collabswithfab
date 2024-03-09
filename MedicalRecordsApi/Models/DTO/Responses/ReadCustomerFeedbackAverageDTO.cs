using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadCustomerFeedbackAverageDto
	{
        public string Month { get; set; }
		public int ExcellentPercentage { get; set; }
		public int JustOkPercentage { get; set; }
		public int PoorPercentage { get; set; }
	}
}
