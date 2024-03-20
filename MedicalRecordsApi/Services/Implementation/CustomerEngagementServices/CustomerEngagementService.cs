using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MedicalRecordsApi.Services.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using MedicalRecordsApi.Models.DTO;

namespace MedicalRecordsApi.Services.Implementation.CustomerEngagementServices
{
	public class CustomerEngagementService : ICustomerEngagementService
	{
		#region config
		private readonly IMapper _mapper;
		private readonly MedicalRecordDbContext _dbContext;
		private readonly IGenericRepository<Employee> _employeeRepository;
		private readonly IGenericService<ReadCustomerFeedbackDto> _genericService;
		private readonly IGenericRepository<CustomerFeedback> _customerFeedbackRepository;
		private readonly IConfiguration _configuration;

        public CustomerEngagementService(MedicalRecordDbContext dbContext, IGenericRepository<CustomerFeedback> customerFeedbackRepository, IConfiguration configuration, IGenericRepository<Employee> employeeRepository, IGenericService<ReadCustomerFeedbackDto> genericService, IMapper mapper)
        {
            _dbContext = dbContext;
            _customerFeedbackRepository = customerFeedbackRepository;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _genericService = genericService;
            _mapper = mapper;
        }
        #endregion

        public async Task<ServiceResponse<string>> AddCustomerFeedbackAsync(CreateCustomerFeedbackDto customerFeedbackDto)
		{
			if (customerFeedbackDto == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var employee = await _employeeRepository.Query()
											.FirstOrDefaultAsync(x => x.Id == customerFeedbackDto.EmployeeId);

			if (employee == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			var feedback = _mapper.Map<CustomerFeedback>(customerFeedbackDto);

			var result = await _customerFeedbackRepository.CreateAsync(feedback);

			return new ServiceResponse<string>("Successful", (InternalCode)result);
		}

		public ServiceResponse<PaginatedList<ReadCustomerFeedbackDto>> GetFeedbackDetailsAsync(int userId, ReviewSource source, string month, int pageIndex = 1, int pageSize = 5)
		{
			// Parse the month string to a Month enum (assumes valid month string)
			Month monthEnum = (Month)Enum.Parse(typeof(Month), month, true);

			// Construct the first day of the specified month and year
			DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, (int)monthEnum, 1, 0, 0, 0);

			// Construct the last day of the specified month and year
			var endDate = firstDayOfMonth.AddMonths(1);

			IQueryable<CustomerFeedback> feedbacks = _customerFeedbackRepository.Query()
														.AsNoTracking()
														.Where(x => x.EmployeeId == userId && x.Source == source && x.CreatedAt >= firstDayOfMonth && x.CreatedAt < endDate);

			IQueryable<ReadCustomerFeedbackDto> data = feedbacks
				.ProjectTo<ReadCustomerFeedbackDto>(_mapper.ConfigurationProvider);

			Expression<Func<ReadCustomerFeedbackDto, string>> expression = x => x.Comments;

			PaginatedList<ReadCustomerFeedbackDto> result = _genericService.SortPaginateByText(pageIndex, pageSize, data, expression, Order.Asc);

			return new ServiceResponse<PaginatedList<ReadCustomerFeedbackDto>>(result, InternalCode.Success);
		}
		
		public async Task<ServiceResponse<ReadCustomerFeedbackAverageDto>> GetMonthlyAverageAsync(int userId, ReviewSource source, string month)
		{
			// Parse the month string to a Month enum (assumes valid month string)
			Month monthEnum = (Month)Enum.Parse(typeof(Month), month, true);

			// Construct the first day of the specified month and year
			DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, (int)monthEnum, 1, 0, 0, 0);

			// Construct the last day of the specified month and year
			var endDate = firstDayOfMonth.AddMonths(1);

			var feedbacks = await _customerFeedbackRepository.Query().AsNoTracking()
															 .Where(x => x.EmployeeId == userId && x.Source == source && x.CreatedAt >= firstDayOfMonth && x.CreatedAt < endDate)
															 .GroupBy(x => x.Rating)
															 .Select(g => new FeedbackDataDto()
															 {
															 	Rating = g.Key,
															 	Count = g.Count()
															 }).ToListAsync();

			int totalRatings = feedbacks.Sum(r => r.Count);

			var averageFeedback = new ReadCustomerFeedbackAverageDto
			{
				Month = month,
				ExcellentPercentage = CalculatePercentage(ReviewRating.Excellent, feedbacks, totalRatings),
				JustOkPercentage = CalculatePercentage(ReviewRating.JustOk, feedbacks, totalRatings),
				PoorPercentage = CalculatePercentage(ReviewRating.Poor, feedbacks, totalRatings)
			};

			return new ServiceResponse<ReadCustomerFeedbackAverageDto>(averageFeedback, InternalCode.Success);
		}

		#region Helpers
		private int CalculatePercentage(ReviewRating rating, List<FeedbackDataDto> ratings, int totalRatings)
		{

			var ratingCount = ratings.FirstOrDefault(r => r.Rating == rating)?.Count ?? 0;
			return totalRatings > 0 ? (int)Math.Round((double)ratingCount / totalRatings * 100) : 0;
		}
		// Enum to represent months
		enum Month
		{
			January = 1,
			February,
			March,
			April,
			May,
			June,
			July,
			August,
			September,
			October,
			November,
			December
		}
		#endregion
	}
}
