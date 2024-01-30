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
using MedicalRecordsRepository.DTO.FacilityDto;
using MedicalRecordsApi.Constants;
using MedicalRecordsData.Enum;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadBedDetailsDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/facilities/beds/assignedtodoctor
        public async Task<IActionResult> Get()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            ServiceResponse<IEnumerable<ReadBedDetailsDTO>> result = await _service.GetBedsAssignedToDoctor(userId);

            return result.FormatResponse();
        }

        //2. GetBedStatus
        /// <summary>
        /// This gets bed information. If free, or occupied
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        [HttpGet]
        [Route("beds")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadBedDetailsDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/facilities/beds
        public async Task<IActionResult> GetBedDetails()
        {
            ServiceResponse<IEnumerable<ReadBedDetailsDTO>> result = await _service.GetBedStatus();

            return result.FormatResponse();
        }

        /// <summary>
        /// method used to assigning bed space
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("assign-bed")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<AssignBedRequestDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> AssignBed(AssignBedRequestDto assignBedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("UserId")?.Value;
            string userRole = User.FindFirst("AccessRoleId")?.Value;
            int UserId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                UserId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                ServiceResponse<string> result = await _service.AssignBed(assignBedDto,UserId);

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        /// <summary>
        /// used to remove assigned bespace
        /// </summary>
        /// <param name="assignBedDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("free-bedspace")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<AssignBedRequestDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> FreeBedSpace(int bedSpaceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("UserId")?.Value;
            string userRole = User.FindFirst("AccessRoleId")?.Value;
            int UserId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                UserId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                ServiceResponse<string> result = await _service.FreeBedSpace(bedSpaceId, UserId);

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
    }
}
