﻿using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces;
using MedicalRecordsApi.Services.Abstract.DashBoardInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using MedicalRecordsApi.Models.DTO.Request.Enums;
using MedicalRecordsData.Enum;
using MedicalRecordsApi.Services.Abstract.EmployeeInterfaces;
using MedicalRecordsApi.Constants;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.Interfaces;
using System.Linq;

namespace MedicalRecordsApi.Controllers.DashBoardEndpoints
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashBoardService _service;
        private readonly IEmployeeService _employeeService;
        private readonly IGenericRepository<Patient> _genericRepositoryPatient;

        public DashboardController(IDashBoardService service, IEmployeeService employeeService, IGenericRepository<Patient> genericRepositoryPatient)
        {
            _service = service;
            _employeeService = employeeService;
            _genericRepositoryPatient = genericRepositoryPatient;
        }

        //1. GetAssignedPatientsCount-Waiting or All
        /// <summary>
        /// This gets the number of patients assigned to a particular doctor from time till date or waiting to be attended to
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        [HttpGet]
        [Route("assignedtodoctor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<long>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/assignedtodoctor
        public async Task<IActionResult> Get([FromQuery] PatientCareStatus status = PatientCareStatus.All)
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<long> result = await _service.GetAssignedPatientsCountAsync(employeeId.Data, status);

            return result.FormatResponse();
        }

        //2. GetAdmittedPatientsCount
        /// <summary>
        /// This gets the number of patients admitted for a particular doctor
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        [HttpGet]
        [Route("doctor/admittedpatients")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<long>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/doctor/admittedpatients
        public async Task<IActionResult> Get()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<long> result = await _service.GetAdmittedPatientsCountAsync(employeeId.Data);

            return result.FormatResponse();
        }

        //3. GetInPatientOutPatientPatientsCount
        /// <summary>
        /// This gets the number of patients taken care of either inpatient or outpatient
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        [HttpGet]
        [Route("administered")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<long>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/administered
        public async Task<IActionResult> Get([FromQuery] PatientCareType careType = PatientCareType.OutPatient)
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<long> result = await _service.GetInPatientOutPatientPatientsCountAsync(employeeId.Data, careType);

            return result.FormatResponse();
        }


        //4. InPatientOutPatientDataAndPercentages
        /// <summary>
        /// This gets the number of patients data for inpatient data or outpatient data
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{ReadPatientCareTypeDto}"/> object.</returns>
        [HttpGet]
        [Route("patientcaretypedata")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientCareTypeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/patientcaretypedata
        public async Task<IActionResult> GetPatientCareTypeData()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<ReadPatientCareTypeDto> result = await _service.InPatientOutPatientDataAndPercentagesAsync(employeeId.Data);

            return result.FormatResponse();
        }

        //5. PatientAdmission = Check for day through visit
        /// <summary>
        /// This gets the number of patients admitted per hour
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{PatientByGender}}"/> object.</returns>
        [HttpGet]
        [Route("admission")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/admission
        public async Task<IActionResult> GetPatientAdmission()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<IEnumerable<ReadPatientAdmissionDto>> result = await _service.PatientAdmissionAsync(employeeId.Data);

            return result.FormatResponse();
        }

        //6. PatientByGender = Get admitted patients and check their genders
        /// <summary>
        /// This gets the number of patients by gender
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{PatientByGender}"/> object.</returns>
        [HttpGet]
        [Route("gender")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<ReadPatientByGenderDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/gender
        public async Task<IActionResult> GetPatientByGender()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<ReadPatientByGenderDto> result = await _service.PatientByGenderAsync(employeeId.Data);

            return result.FormatResponse();
        }

        //7. GetPatientByHMO
        /// <summary>
        /// This gets the number of patients by thats got HMO
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        [HttpGet]
        [Route("hmo-patient")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<long>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/gender
        public async Task<IActionResult> GetPatientWithHmo()
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<long> result = await _service.GetPatientByHmoAsync(employeeId.Data);

            return result.FormatResponse();
        }
        //1. GetAssignedPatientsCount-Waiting or All
        /// <summary>
        /// This gets the number of patients assigned to a particular doctor from time till date or waiting to be attended to
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        [HttpGet]
        [Route("assignedtonurse")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<long>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        // GET api/dashboard/assignedtodoctor
        public async Task<IActionResult> GetPatientAssignedToNurse([FromQuery] PatientCareStatus status = PatientCareStatus.All)
        {
            int userId = int.Parse(User.FindFirst("Id").Value);

            //Get Employee Id
            var employeeId = await _employeeService.GetEmployeeId(userId);

            ServiceResponse<long> result = await _service.GetAssignedPatientsCountAsync(employeeId.Data, status);

            return result.FormatResponse();
        }
        [HttpGet("AvaliableStaff/{clinicId}")]
        public IActionResult AvalaibleStaff([FromRoute] int clinicId)
        {

            // caling the service here
            var response = _service.AvaliableStaff(clinicId);
            return response.FormatResponse();
        }
        [HttpGet("AllOutPatientAndInPatientCount")]
        public IActionResult AllOutPatientAndInPatientCount()
        {
            // caling the service here
            var response = _service.AllOutPatientAndInPatientCount();
            return response.FormatResponse();
        }
        [HttpGet("AllPatientCount")]
        public IActionResult AllPatientCount()
        {
            // caling the service here
            var totalCount = _genericRepositoryPatient.GetAll().Count();
            var response = new ServiceResponse<int>(totalCount, InternalCode.Success, ServiceErrorMessages.Success);
            return response.FormatResponse();
        }
    }
}
