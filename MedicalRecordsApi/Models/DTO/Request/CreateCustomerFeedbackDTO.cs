using MedicalRecordsData.Entities.MedicalRecordsEntity;

namespace MedicalRecordsApi.Models.DTO.Request
{
	public class CreateCustomerFeedbackDTO
	{
		public ReviewRating Rating { get; set; }
		public string Comments { get; set; }
		public ReviewSource Source { get; set; }
		public int EmployeeId { get; set; }
	}
}
