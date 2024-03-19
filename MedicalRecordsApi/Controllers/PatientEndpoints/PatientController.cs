
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.EmployeeInterfaces;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Implementation.EmployeeServices;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.DTO.MedicalDto;
using MedicalRecordsRepository.DTO.PatientDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers.PatientEndpoints
{
    [Route("api/patients")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly IEmployeeService _employeeService;

        public PatientController(IPatientService service, IEmployeeService employeeService)
        {
            _service = service;
            _employeeService = employeeService;
        }

        //1. GetAssignedWaitingPatients
        /// <summary>
        /// This gets the patients assigned to a particular doctor
        /// </summary>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{AssignedPatientsDTO}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
		[Route("assignedtodoctor")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<AssignedPatientsDto>>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/assignedtodoctor
		public async Task<IActionResult> Get()
		{
			int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<IEnumerable<AssignedPatientsDto>> result = await _service.GetAssignedPatientsAsync(employeeId.Data);

            return result.FormatResponse();
        }

		//2. GetPatientData
		/// <summary>
		/// This gets the patients record of a particular patient
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadPatientDTO}"/> object.</returns>
		[HttpGet]
		[Route("{patientId}/data")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientDto>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/data
		public async Task<IActionResult> Get([FromRoute] int patientId)
		{
			ServiceResponse<ReadPatientDto> result = await _service.GetPatientDataAsync(patientId);

            return result.FormatResponse();
        }

		//3. GetNurseNote
		/// <summary>
		/// This gets nurses notes for a particular visit of a patient
		/// </summary>
		/// <param name="patientId"></param>
		/// <param name="visitId"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadNurseNotesDTO}"/> object.</returns>
		[HttpGet]
		[Route("{patientId}/nursenotes/{visitId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadNurseNotesDto>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/nursenotes/2
		public async Task<IActionResult> Get([FromRoute] int patientId, [FromRoute] int visitId)
		{
			ServiceResponse<ReadNurseNotesDto> result = await _service.GetNurseNoteAsync(patientId, visitId);

			return result.FormatResponse();
		}

		//4. AddToPatientNote
        /// <summary>
        /// This adds to the patient note in the patients record
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="patientNoteDto"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{string}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpPost]
		[Route("{patientId}/addpatientnote")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// POST api/patients/5/addpatientnote
		public async Task<IActionResult> Post([FromRoute] int patientId, [FromBody] CreatePatientNoteDto patientNoteDto)
		{
			ServiceResponse<string> result = await _service.AddToPatientNoteAsync(patientId, patientNoteDto);

            return result.FormatResponse();
        }

        //5. ReferPatient
        /// <summary>
        /// This adds to the lab table thereby referring the patient to the lab
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <param name="labReferDto"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{string}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpPost]
        [Route("{patientId}/visit/{visitId}/labrequest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // POST api/patients/5/visit/3/labrequest
        public async Task<IActionResult> Post([FromRoute] int patientId, [FromRoute] int visitId, [FromBody] CreateLabReferDto labReferDto)
        {
            ServiceResponse<string> result = await _service.ReferPatientAsync(patientId, visitId, labReferDto);

            return result.FormatResponse();
        }

        //6. AddPrescription
        /// <summary>
        /// This adds to the prescription data in the patients record
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="prescriptionDto"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{string}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpPost]
		[Route("{patientId}/addtreatmentprescription")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
        // POST api/patients/5/addtreatmentprescription
        public async Task<IActionResult> Post([FromRoute] int patientId, [FromBody]CreatePatientPrescriptionDto prescriptionDto)
		{
			ServiceResponse<string> result = await _service.AddPrescriptionAsync(patientId, prescriptionDto);

			return result.FormatResponse();
		}

        //7. GetLabNote
        /// <summary>
        /// This gets the lab report of a patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="labrequestId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{ReadPatientLabReport}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/labrequest/{labrequestId}/report")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientLabReport>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/labrequest/1/report
        public async Task<IActionResult> GetLabReport([FromRoute] int patientId, [FromRoute] int labrequestId)
        {
            ServiceResponse<ReadPatientLabReport> result = await _service.GetLabReportAsync(patientId, labrequestId);

            return result.FormatResponse();
        }

        //8. GetAllAdmissionHistory
        /// <summary>
        /// This gets the patients admission history for quick evaluation
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadVisitHistoryDTO}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
		[Route("{patientId}/admission/history")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/admission/history
		public async Task<IActionResult> GetHistory([FromRoute] int patientId)
		{
			ServiceResponse<IEnumerable<ReadVisitHistoryDto>> result = await _service.GetAllAdmissionHistoryAsync(patientId);
            return result.FormatResponse();
        }

        //9. GetContactDetails
        /// <summary>
        /// This gets the patients contact details
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{ReadContactDetailsDto}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/contact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadContactDetailsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/contact
        public async Task<IActionResult> GetContactDetails([FromRoute] int patientId)
        {
            ServiceResponse<ReadContactDetailsDto> result = await _service.GetContactDetailsAsync(patientId);
            return result.FormatResponse();
        }

        //10. GetEmergencyContactDetails
        /// <summary>
        /// This gets the patients emergency contact details
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{ReadEmergencyContactDetailsDto}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/emergencycontact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadEmergencyContactDetailsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/emergencycontact
        public async Task<IActionResult> GetEmergencyContactDetails([FromRoute] int patientId)
        {
            ServiceResponse<ReadEmergencyContactDetailsDto> result = await _service.GetEmergencyContactDetailsAsync(patientId);
            return result.FormatResponse();
        }

        //11. GetMedicalRecord
        /// <summary>
        /// This gets the patients medical record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadMedicalRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/medicalrecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadMedicalRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/medicalrecord
        public IActionResult GetMedicalRecord([FromRoute] int patientId)
        {
            ServiceResponse<IEnumerable<ReadMedicalRecordDto>> result = _service.GetMedicalRecordAsync(patientId);
            return result.FormatResponse();
        }

        //12. GetImmunizationRecord
        /// <summary>
        /// This gets the patients immunization record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadImmunizationRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/immunizationrecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/immunizationrecord
        public async Task<IActionResult> GetImmunizationRecordAsync([FromRoute] int patientId)
        {
            ServiceResponse<IEnumerable<ReadImmunizationRecordDto>> result = await _service.GetImmunizationRecordAsync(patientId);
            return result.FormatResponse();
        }

        //13. GetVisitRecord
        /// <summary>
        /// This gets the patients visit record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadVisitHistoryDto}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/visitrecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadVisitHistoryDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/visitrecord
        public async Task<IActionResult> GetVisitRecordAsync([FromRoute] int patientId)
        {
            ServiceResponse<IEnumerable<ReadVisitHistoryDto>> result = await _service.GetVisitRecordAsync(patientId);
            return result.FormatResponse();
        }

        //14. GetTreatmentRecord
        /// <summary>
        /// This gets the patients treatment record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadTreatmentRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        [HttpGet]
        [Route("{patientId}/treatmentrecord")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/treatmentrecord
        public async Task<IActionResult> GetTreatmentRecord([FromRoute] int patientId)
        {
            ServiceResponse<IEnumerable<ReadTreatmentRecordDto>> result = await _service.GetTreatmentRecordAsync(patientId);
            return result.FormatResponse();
        }

        
        /// <summary>
        /// used to add the patient to the nurse or doctors queue
        /// </summary>
        /// <param name="createPatientDto"></param>
        /// <returns></returns>
        [HttpPost("AddPatient")]
        public async Task<IActionResult> AddPatient([FromBody] CreatePatientRequestDto createPatientDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.AddPatient(createPatientDto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
                
        }

        [HttpPost("UpdateContact")]
        public async Task<IActionResult> InsertOrUpdateContact(UpdateContactDto contactDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.UpdateContact(contactDto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpPost("EmergerncyContact")]
        public async Task<IActionResult> InsertOrUpdateEmergerncyContact(UpdateEmergencyContactDto emergencyContact)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.UpdateEmergencyContact(emergencyContact, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpPost("AddMedicalReport")]
        public async Task<IActionResult> AddMedicalReport(MedicalRecordsDto medicalReportDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.AddMedicalReport(medicalReportDto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpPost("AddVisitationRecords")]
        public async Task<IActionResult> AddVisitationRecords(PatientsVisitsDto patientsVisitsDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.AddPatientVistsRecords(patientsVisitsDto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpPost("AddImmunizationRecords")]
        public async Task<IActionResult> AddImmunizationRecords(ImmunizationDto immunizationObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userId = 0;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.AddImmunizationRecords(immunizationObj, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }
        [HttpDelete("DeleteImmunizationRecordsById")]
        public async Task<IActionResult> DeleteImmunizationRecords(int immunizationId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.DeleteImmunizationRecord(immunizationId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpDelete("DeleteVisitationById")]
        public async Task<IActionResult> DeleteVisitationRecords(int visitationId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.DeleteImmunizationRecord(visitationId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }

        }

        [HttpDelete("DeleteMedicalRecordById")]
        public async Task<IActionResult> DeleteMedicalRecords(int medicalRecordsId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.DeleteMedicalReport(medicalRecordsId);
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
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllMedicalRecordByPatientId")]
        public async Task<IActionResult> GetAllMedicalRecordsByPatientId(int patientId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllMedicalReportByPatientId(patientId);
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
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllVisitationRecordByPatientId")]
        public async Task<IActionResult> GetAllVisitationRecordByPatientId(int patientId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllVisitationByPatientId(patientId);
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
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetAllImmunizationRecordByPatientId")]
        public async Task<IActionResult> GetAllImmunizationRecordByPatientId(int patientId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse || userRoleId == (int)MedicalRole.Doctors)
            {
                // caling the service here
                var response = await _service.GetAllImmunizatiobByPatientId(patientId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }

        [HttpPut("updatemedicalstaffbypatientId")]
        public async Task<IActionResult> UpdateMedicalStaffByPatientId(UpdateMedicalStaffDto UpdateMedicalStaffDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ModelState });
            }
            string username = User.FindFirst("id")?.Value;
            string userRole = User.FindFirst("RoleId")?.Value;
            int userRoleId = 0;
            int userId = 0;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Failed, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
            if (int.TryParse(username, out int convertedUserId))
            {
                userId = convertedUserId;
            }
            if (int.TryParse(userRole, out int convertedUserRoleId))
            {
                userRoleId = convertedUserRoleId;
            }
            if (userRoleId == (int)MedicalRole.Nurse)
            {
                // caling the service here
                var response = await _service.UpdateMedicalStaffByPatientId(UpdateMedicalStaffDto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }
        [HttpGet("AllPatient")]
        public IActionResult GetAllPatient([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
           
            // caling the service here
            var response = _service.GetAllPatient(pageIndex, pageSize);
            return response.FormatResponse();
        }
        [HttpGet("AllPatientById")]
        public IActionResult GetAllPatientById([FromQuery] int patientId)
        {

            // caling the service here
            var response = _service.GetAllPatientById(patientId);
            return response.FormatResponse();
        }
        [HttpGet("AllNurse")]
        public IActionResult GetAllNurses([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {

            // caling the service here
            var response = _service.GetAllNurses(pageIndex, pageSize);
            return response.FormatResponse();
        }
        [HttpGet("GetAllMedicalTypes")]
        public IActionResult GetAllMedicalTypes([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var enumValues = Enum.GetValues(typeof(MedicalRecordType));
            var listTypes = new List<(int position, string value)>();
            for (int i = 0; i < enumValues.Length; i++)
            {
                var enumName = Enum.GetName(typeof(MedicalRecordType), enumValues.GetValue(i));
                listTypes.Add(( i, enumName));
            } 
            // caling the service here
            return new ServiceResponse<List<(int position, string value)>>
                   (listTypes, InternalCode.Success, ServiceErrorMessages.Success)
                   .FormatResponse();
        }
    }
}
