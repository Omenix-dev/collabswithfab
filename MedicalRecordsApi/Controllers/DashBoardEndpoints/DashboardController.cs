using MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces;
using MedicalRecordsApi.Services.Abstract.DashBoardInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalRecordsApi.Controllers.DashBoardEndpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashBoardService _service;

        public DashboardController(IDashBoardService service)
        {
            _service = service;
        }
    }
}
