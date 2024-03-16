using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Abstract.ReferralInterfaces;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO.FacilityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using MedicalRecordsRepository.DTO.ReferralDto;

namespace MedicalRecordsApi.Controllers.ReferralsEndpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralServices _service;
        public ReferralsController(IReferralServices service)
        {
            _service = service;
        }

        /// <summary>
        /// used to add note to the patient referral model
        /// </summary>
        /// <param name="ReferralNoteDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-referral-notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> AddReferralNotes(ReferralNoteDto ReferralNoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
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
                ServiceResponse<string> result = await _service.AddReferralNote(ReferralNoteDto, UserId);

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }

        /// <summary>
        /// used to add note to the patient referral model
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete-referral-notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> DeleteReferralNotes(int PatientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
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
                ServiceResponse<string> result = await _service.DeleteReferralNote(PatientId);

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }

        /// <summary>
        /// get all the patient and their most recent daignosis
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll-Referral-notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<GetPatientReferralDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> GetAllReferral()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
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
                var result = await _service.GetAllReferral();

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }

        /// <summary>
        /// get all the patient and their most recent daignosis
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get-Referral-notes-byPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetPatientReferralDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> GetAllReferralByPatientId(int PatientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
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
                var result = await _service.GetAllReferralByPatientId(PatientId);

                return result.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        /// <summary>
        /// updates the patient by NoteId
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update-Referral-notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public async Task<IActionResult> UpdateNotebyId(ReferralNoteDto ReferralNoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
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
                var result = await _service.UpdateReferralNote(ReferralNoteDto, UserId);

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
