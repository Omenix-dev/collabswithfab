using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers.PatientEndpoints
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service)
        {
            _service = service;
        }

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
		// GET api/patients/assignedtodoctor
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
		[Route("{patientId}/data")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientDTO>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/data
		public async Task<IActionResult> Get([FromRoute] int patientId)
		{
			ServiceResponse<ReadPatientDTO> result = await _service.GetPatientDataAsync(patientId);

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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadNurseNotesDTO>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/nursenotes/2
		public async Task<IActionResult> Get([FromRoute] int patientId, [FromRoute] int visitId)
		{
			ServiceResponse<ReadNurseNotesDTO> result = await _service.GetNurseNoteAsync(patientId, visitId);

			return result.FormatResponse();
		}

		//4. AddToPatientNote
		/// <summary>
		/// This adds to the patient note in the patients record
		/// </summary>
		/// <param name="patientId"></param>
		/// <param name="patientNoteDTO"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
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
		public async Task<IActionResult> Post([FromRoute] int patientId, [FromBody] CreatePatientNoteDTO patientNoteDTO)
		{
			ServiceResponse<string> result = await _service.AddToPatientNoteAsync(patientId, patientNoteDTO);

			return result.FormatResponse();
		}

        //5. ReferPatient
        /// <summary>
        /// This adds to the lab table thereby referring the patient to the lab
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <param name="labReferDTO"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        [HttpPost]
        [Route("{patientId}/visit/{visitId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // POST api/patients/5/visit/3
        public async Task<IActionResult> Post([FromRoute] int patientId, [FromRoute] int visitId, [FromBody] CreateLabReferDTO labReferDTO)
        {
            ServiceResponse<string> result = await _service.ReferPatientAsync(patientId, visitId, labReferDTO);

            return result.FormatResponse();
        }

        //6. AddPrescription
        /// <summary>
        /// This adds to the prescription data in the patients record
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="prescriptionDTO"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        [HttpPost]
		[Route("{patientId}/addprescription")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// POST api/patients/5/addprescription
		public async Task<IActionResult> Post([FromRoute] int patientId, [FromBody]CreatePatientPrescriptionDTO prescriptionDTO)
		{
			ServiceResponse<string> result = await _service.AddPrescriptionAsync(patientId, prescriptionDTO);

			return result.FormatResponse();
		}

        //7. GetLabNote
        /// <summary>
        /// This gets the lab note of a patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="labId"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        [HttpGet]
        [Route("{patientId}/lab/{labId}/note")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/patients/5/lab/1/note
        public async Task<IActionResult> GetLabNote([FromRoute] int patientId, [FromRoute] int labId)
        {
            ServiceResponse<string> result = await _service.GetLabNoteAsync(patientId, labId);

            return result.FormatResponse();
        }

        //8. GetAllAdmissionHistory
        /// <summary>
        /// This gets the patients admission history for quick evaluation
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadVisitHistoryDTO}}"/> object.</returns>
        [HttpGet]
		[Route("{patientId}/admission/history")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patients/5/admission/history
		public async Task<IActionResult> GetHistory([FromRoute] int patientId)
		{
			ServiceResponse<IEnumerable<ReadVisitHistoryDTO>> result = await _service.GetAllAdmissionHistoryAsync(patientId);

			return result.FormatResponse();
		}
	}
}
