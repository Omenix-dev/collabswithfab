using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request.Enums;
using MedicalRecordsApi.Services.Abstract.EmployeeInterfaces;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;

        public EmployeeService(IGenericRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ServiceResponse<int>> GetEmployeeId(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<int>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var employeeId = await _employeeRepository.Query()
                                          .AsNoTracking()
                                          .Where(x => x.UserId == userId)
                                          .Select(x => x.Id)
                                          .FirstOrDefaultAsync();

            return new ServiceResponse<int>(employeeId, InternalCode.Success);
        }
    }
}
