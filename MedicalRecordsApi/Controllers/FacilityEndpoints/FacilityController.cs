using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.FacilityInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MedicalRecordsApi.Controllers.FacilityEndpoints
{
    [Route("api/facilities")]
    [ApiController]
    [Authorize]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _service;

        public FacilityController(IFacilityService service)
        {
            _service = service;
        }

        //1. GetBedsAssignedToDoctor
        /// <summary>
        /// This gets bed information. If free, If assigned to doctor. Etc
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        [HttpGet]
        [Route("beds/assignedtodoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadBedDetailsDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/facilities/beds/assignedtodoctor
        public async Task<IActionResult> Get()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            ServiceResponse<IEnumerable<ReadBedDetailsDto>> result = await _service.GetBedsAssignedToDoctor(userId);

            return result.FormatResponse();
        }

        //2. GetBedStatus
        /// <summary>
        /// This gets bed information. If free, or occupied
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        [HttpGet]
        [Route("beds")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadBedDetailsDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/facilities/beds
        public async Task<IActionResult> GetBedDetails()
        {
            ServiceResponse<IEnumerable<ReadBedDetailsDto>> result = await _service.GetBedStatus();

            return result.FormatResponse();
        }
    }
}
