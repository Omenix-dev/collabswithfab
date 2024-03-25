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
using System;

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
        public async Task<IActionResult> AddReferralNotes(ReferralDto ReferralNoteDto)
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
                ServiceResponse<string> result = await _service.AddReferral(ReferralNoteDto, UserId);

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
                ServiceResponse<object> result = await _service.RemoveReferredPatient(PatientId);

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
        [Route("GetAll-Referral-notes/{clinicId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<GetPatientReferralDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public IActionResult GetAllReferral([FromRoute] int clinicId, [FromQuery] int pageIndex, int PageSize)
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
                var result = _service.GetAllReferral(clinicId, pageIndex, PageSize);

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
        [Route("Get-Referral-notes-byPatientId/{ClinicId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetPatientReferralDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // remove a patient from a bed
        public IActionResult GetAllReferralByPatientId([FromRoute] int ClinicId, [FromQuery] int PatientId)
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
                var result = _service.GetAllReferralByPatientId(ClinicId, PatientId);

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
        [HttpGet("GetAllAcceptanceStatus")]
        public IActionResult GetAllAcceptanceStatus()
        {
            var enumValues = Enum.GetValues(typeof(AcceptanceStatus));
            var listTypes = new List<object>();
            for (int i = 1; i <= enumValues.Length; i++)
            {
                var enumName = Enum.GetName(typeof(AcceptanceStatus), enumValues.GetValue(i - 1));
                listTypes.Add(new { index = i, value = enumName });
            }
            // caling the service here
            return new ServiceResponse<List<object>>
                   (listTypes, InternalCode.Success, ServiceErrorMessages.Success)
                   .FormatResponse();
        }
    }
}
