using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;

namespace MedicalRecordsApi.Models.DTO.Request
{
	public class CreateCustomerFeedbackDto
	{
		public ReviewRating Rating { get; set; }
		public string Comments { get; set; }
		public ReviewSource Source { get; set; }
		public int EmployeeId { get; set; }
	}
}
