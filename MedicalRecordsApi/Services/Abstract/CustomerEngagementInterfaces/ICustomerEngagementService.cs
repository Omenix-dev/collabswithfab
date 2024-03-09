using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces
{
	public interface ICustomerEngagementService
	{
		/// <summary>
		/// This adds to the doctor or nurse customer feedback table
		/// </summary>
		/// <param name="customerFeedbackDto"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
		Task<ServiceResponse<string>> AddCustomerFeedbackAsync(CreateCustomerFeedbackDto customerFeedbackDto);
		/// <summary>
		/// This gets the monthly average feedback details
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadCustomerFeedbackAverageDTO}"/> object.</returns>
		Task<ServiceResponse<ReadCustomerFeedbackAverageDto>> GetMonthlyAverageAsync(int userId, ReviewSource source, string month);
		/// <summary>
		/// This gets paginated customer engagement details
		/// </summary>
		/// <param name="source"></param>
		/// <param name="month"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns>Returns a <see cref="ServiceResponse{PaginatedList{ReadCustomerFeedbackDTO}}"/> object.</returns>
		ServiceResponse<PaginatedList<ReadCustomerFeedbackDto>> GetFeedbackDetailsAsync(int userId, ReviewSource source, string month, int pageIndex, int pageSize);
	}
}
