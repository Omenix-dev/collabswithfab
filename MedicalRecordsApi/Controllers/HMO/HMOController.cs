using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.HMOInterface;
using MedicalRecordsData.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers.HMO
{
    [Route("api/[controller]")]
    [ApiController]
    public class HMOController : ControllerBase
    {
        private readonly IHMOService _service;
        public HMOController(IHMOService hMOService)
        {
            _service = hMOService;
        }

        /// <summary>
        /// used to add the patient HMO Plan
        /// </summary>
        /// <param name="PatientHMODto"></param>
        /// <returns></returns>
        [HttpPost("AddHMOPlan")]
        public async Task<IActionResult> AddHMOPlan([FromBody] PatientHMODto PatientHMODto)
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
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Unauthorized, "he user role is empty");
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
                var response = await _service.AddPatientHMOAsync(PatientHMODto, userId);
                return response.FormatResponse();
            }
            else
            {
                var value = new ServiceResponse<string>("the user is not authorized", InternalCode.Unauthorized, ServiceErrorMessages.OperationFailed);
                return value.FormatResponse();
            }
        }

        /// <summary>
        /// used to update the patient HMO Plan
        /// </summary>
        /// <param name="UpdatePatientHMODto"></param>
        /// <returns></returns>
        [HttpPost("UpdateHMOPlan")]
        public async Task<IActionResult> UpdateHMOPlan([FromBody] UpdatePatientHMODto UpdatePatientHMODto)
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
                var value = new ServiceResponse<string>("the user role is empty", InternalCode.Unauthorized, "he user role is empty");
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
                var response = await _service.UpdatePatientHMOAsync(UpdatePatientHMODto, userId);
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
