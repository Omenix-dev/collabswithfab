using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;

namespace MedicalRecordsApi.Models.DTO
{
	public class FeedbackDataDTO
	{
		public ReviewRating Rating { get; set; }
		public int Count { get; set; }
	}
}
