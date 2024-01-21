﻿using MedicalRecordsApi.Models.DTO.Request;
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
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service)
        {
            _service = service;
        }

		//Patients
		//1. GetAssignedPatients
		/// <summary>
		/// This gets the patients assigned to a particular doctor
		/// </summary>
		/// <returns>Returns a <see cref="ServiceResponse{List{AssignedPatientsDTO}}"/> object.</returns>
		[HttpGet]
		[Route("assignedtodoctor")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<List<AssignedPatientsDTO>>))]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		// GET api/patient/assignedtodoctor
		public async Task<IActionResult> Get()
		{
			int userId = int.Parse(User.FindFirst("Id").Value);

			ServiceResponse<List<AssignedPatientsDTO>> result = await _service.GetAssignedPatientsAsync(userId);

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

		//5. ReferPatient

		//6. AddPrescription
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
		public async Task<IActionResult> Post([FromBody]CreatePatientPrescriptionDTO prescriptionDTO)
		{
			ServiceResponse<string> result = await _service.AddPrescriptionAsync(prescriptionDTO);

			return result.FormatResponse();
		}

		//7. GetLabNote
		//8. GetAllAdmissionHistory

	}
}
