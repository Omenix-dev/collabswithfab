using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.EmployeeInterfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// This gets the employee id of a user
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{int}"/> object.</returns>
        Task<ServiceResponse<int>> GetEmployeeId(int userId);
    }
}
