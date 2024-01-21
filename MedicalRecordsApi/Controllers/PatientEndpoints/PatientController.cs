
using MedicalRecordsApi.Models;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO.AuthDTO;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
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

        [HttpPost("CreatePatientProfile")]
        public async Task<IActionResult> CreatePatientProfile(CreatePatientProfileDto CreateProfileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Validation failed", Errors = ModelState });
                }
                string username = User.FindFirst("UserId")?.Value;
                string userRole = User.FindFirst("AccessRoleId")?.Value;
                int UserId = 0;
                int userRoleId = 0;
                APIResponse response = null;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
                {
                    return Ok(new
                    {
                        Code = 2,
                        Status = "Access_Denied",
                        Message = $"User has no access to this module"
                    });
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
                    response = await _service.CreatePatientProfile(CreateProfileDto, UserId);
                }
                else
                {
                    return Ok(new
                    {
                        Code = 2,
                        Status = "Access_Denied",
                        Message = $"User has no access"
                    });
                }

                if (response.StatusCode == "01")
                {
                    return Ok(new
                    {
                        Code = 1,
                        Status = "success",
                        Message = response.ApiMessage,
                        Data = response.Result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Code = 1,
                        Status = "failed",
                        Message = response.ApiMessage
                    }) ;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Failed to get token: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}");
                return StatusCode(500, new
                {
                    Code = 5,
                    Status = "failed",
                    ex.Message
                });
            }
        }
    }
}
