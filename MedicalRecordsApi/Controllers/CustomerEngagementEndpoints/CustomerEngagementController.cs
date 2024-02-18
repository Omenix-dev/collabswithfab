using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using System;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO;
using MedicalRecordsData.Enum;

namespace MedicalRecordsApi.Controllers.CustomerEngagementEndpoints
{
	[Route("api/customerengagements")]
	[ApiController]
	public class CustomerEngagementController : ControllerBase
	{
		private readonly ICustomerEngagementService _service;

		public CustomerEngagementController(ICustomerEngagementService service)
		{
			_service = service;
		}

		//1. AddCustomerFeedback
		/// <summary>
		/// This adds to the doctor or nurse customer feedback table
		/// </summary>
		/// <param name="customerFeedbackDTO"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
		[HttpPost]
		[Route("addfeedback")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// POST api/customerengagements/addfeedback
		public async Task<IActionResult> Post([FromBody] CreateCustomerFeedbackDTO customerFeedbackDTO)
		{
			ServiceResponse<string> result = await _service.AddCustomerFeedbackAsync(customerFeedbackDTO);

			return result.FormatResponse();
		}

		//2. GetFeedbackDetails
		/// <summary>
		/// This gets paginated customer engagement details
		/// </summary>
		/// <param name="source"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns>Returns a <see cref="ServiceResponse{PaginatedList{ReadCustomerFeedbackDTO}}"/> object.</returns>
		[HttpGet]
		[Route("customerengagements/{source}/{month}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<PaginatedList<ReadBedDetailsDTO>>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/customerengagements/5/January
		public IActionResult Get([FromRoute] ReviewSource source, [FromRoute] string month, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
		{
			int userId = int.Parse(User.FindFirst("Id").Value);

			ServiceResponse<PaginatedList<ReadCustomerFeedbackDTO>> result = _service.GetFeedbackDetailsAsync(userId, source, month, pageIndex, pageSize);

			return result.FormatResponse();
		}

		//3. GetMonthlyAverage
		/// <summary>
		/// This gets the monthly average feedback details
		/// </summary>
		/// <param name="source"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadCustomerFeedbackAverageDTO}"/> object.</returns>
		[HttpGet]
		[Route("customerengagements/{source}/{month}/average")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadCustomerFeedbackAverageDTO>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/customerengagements/5/January/average
		public async Task<IActionResult> Get([FromRoute] ReviewSource source, [FromRoute] string month)
		{
			int userId = int.Parse(User.FindFirst("Id").Value);

			ServiceResponse<ReadCustomerFeedbackAverageDTO> result = await _service.GetMonthlyAverageAsync(userId, source, month);

			return result.FormatResponse();
		}
	}
}
