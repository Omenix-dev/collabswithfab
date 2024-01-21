using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
