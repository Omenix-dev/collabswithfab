
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.DTO.MedicalDto;
using MedicalRecordsRepository.DTO.PatientDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers.PatientEndpoints
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly ILogger _logger;
        public PatientController(IPatientService service, ILogger logger)
        {
            _logger = logger;
            _service = service;
        }

        //Patients
        //1. GetAssignedPatients
        /// <summary>
        /// This gets the patients assigned to a particular doctor
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{AssignedPatientsDTO}}"/> object.</returns>
        [HttpGet]
        [Route("assignedtodoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<AssignedPatientsDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patient/assignedtodoctor
        public async Task<IActionResult> Get()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            ServiceResponse<IEnumerable<AssignedPatientsDTO>> result = await _service.GetAssignedPatientsAsync(userId);

            return result.FormatResponse();
        }

        //2. GetPatientData
        /// <summary>
        /// This gets the patients record of a particular patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see cref="ServiceResponse{ReadPatientDTO}"/> object.</returns>
        [HttpGet]
        [Route("data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patient/data/5
        public async Task<IActionResult> Get([FromRoute] int patientId)
        {
            ServiceResponse<ReadPatientDTO> result = await _service.GetPatientDataAsync(patientId);

            return result.FormatResponse();
        }

        //3. GetNurseNote

        //4. AddToPatientNote
        /// <summary>
        /// This adds to the patient note in the patients record
        /// </summary>
        /// <param name="patientNoteDTO"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        [HttpPost]
        [Route("addpatientnote")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // POST api/patient/addpatientnote
        public async Task<IActionResult> Post([FromBody] CreatePatientNoteDTO patientNoteDTO)
        {
            ServiceResponse<string> result = await _service.AddToPatientNoteAsync(patientNoteDTO);

            return result.FormatResponse();
        }

        /// <summary>
        /// This adds to the prescription data in the patients record
        /// </summary>
        /// <param name="prescriptionDTO"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        [HttpPost]
        [Route("addprescription")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // POST api/patient/addprescription
        public async Task<IActionResult> Post([FromBody] CreatePatientPrescriptionDTO prescriptionDTO)
        {
            ServiceResponse<string> result = await _service.AddPrescriptionAsync(prescriptionDTO);

            return result.FormatResponse();
        }






















        /// <summary>
        /// create the profile for the patient user
        /// </summary>
        /// <param name="CreateProfileDto"></param>
        /// <returns></returns>
        [HttpPost("CreatePatientProfile")]
        public async Task<IActionResult> CreatePatientProfile(CreatePatientProfileDto CreateProfileDto)
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
                var value = new ServiceResponse<string>("the user role is empty",InternalCode.Failed,ServiceErrorMessages.OperationFailed);
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.CreatePatientProfile(CreateProfileDto, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            
        }
        /// <summary>
        /// used to add the patient to the nurse or doctors queue
        /// </summary>
        /// <param name="createPatientDto"></param>
        /// <returns></returns>
        [HttpPost("AddPatient")]
        public async Task<IActionResult> AddPatient(CreatePatientRequestDto createPatientDto)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.AddPatient(createPatientDto, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
                
        }

        [HttpPost("UpdateContact")]
        public async Task<IActionResult> UpdateContact(updateContactDto contactDto)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.UpdateContact(contactDto, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpPost("EmergerncyContact")]
        public async Task<IActionResult> EmergerncyContact(UpdateEmergencyContactDto EmergencyContact)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.UpdateEmergencyContact(EmergencyContact, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpPost("AddMedicalReport")]
        public async Task<IActionResult> AddMedicalReport(MedicalRecordsDto MedicalReportDto)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.AddMedicalReport(MedicalReportDto, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpPost("AddVisitationRecords")]
        public async Task<IActionResult> AddVisitationRecords(PatientsVisitsDto PatientsVisitsDto)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.AddPatientVistsRecords(PatientsVisitsDto, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpPost("AddImmunizationRecords")]
        public async Task<IActionResult> AddImmunizationRecords(ImmunizationDto ImmunizationObj)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.AddImmunizationRecords(ImmunizationObj, UserId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpDelete("DeleteImmunizationRecordsById")]
        public async Task<IActionResult> DeleteImmunizationRecords(int ImmunizationId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.DeleteImmunizationRecord(ImmunizationId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpDelete("DeleteVisitationById")]
        public async Task<IActionResult> DeleteVisitationRecords(int VisitationId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.DeleteImmunizationRecord(VisitationId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpDelete("DeleteMedicalRecordById")]
        public async Task<IActionResult> DeleteMedicalRecords(int MedicalRecordsId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.DeleteMedicalReport(MedicalRecordsId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        /// <summary>
        /// get all the patienrt Medical Records
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllMedicalRecordByPatientId")]
        public async Task<IActionResult> GetAllMedicalRecordsByPatientId(int PatientId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllMedicalReportByPatientId(PatientId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        /// <summary>
        /// get all the visitation by patient Id
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllVisitationRecordByPatientId")]
        public async Task<IActionResult> GetAllVisitationRecordByPatientId(int PatientId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllVisitationByPatientId(PatientId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        /// <summary>
        /// get all the immunzation records by the patient Id
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllImmunizationRecordByPatientId")]
        public async Task<IActionResult> GetAllImmunizationRecordByPatientId(int PatientId)
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
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllImmunizatiobByPatientId(PatientId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
    }
}
