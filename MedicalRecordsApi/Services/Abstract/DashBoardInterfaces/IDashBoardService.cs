using MedicalRecordsApi.Models.DTO.Request.Enums;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsData.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.DashBoardInterfaces
{
    public interface IDashBoardService
    {
        /// <summary>
        /// This gets the number of patients admitted for a particular doctor
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        Task<ServiceResponse<long>> GetAdmittedPatientsCountAsync(int userId);
        /// <summary>
        /// This gets the number of patients assigned to a particular doctor from time till date
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        Task<ServiceResponse<long>> GetAssignedPatientsCountAsync(int userId, PatientCareStatus status);
        /// <summary>
        /// This gets the number of patients taken care of either inpatient or outpatient
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        Task<ServiceResponse<long>> GetInPatientOutPatientPatientsCountAsync(int userId, PatientCareType careType);
        /// <summary>
        /// This gets the number of patients by thats got HMO
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{long}"/> object.</returns>
        Task<ServiceResponse<long>> GetPatientByHmoAsync(int userId);
        /// <summary>
        /// This gets the number of patients data for inpatient data or outpatient data
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{ReadPatientCareTypeDto}"/> object.</returns>
        Task<ServiceResponse<ReadPatientCareTypeDto>> InPatientOutPatientDataAndPercentagesAsync(int userId);
        /// <summary>
        /// This gets the number of patients admitted per hour
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{PatientByGender}}"/> object.</returns>
        Task<ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>> PatientAdmissionAsync(int userId);
        /// <summary>
        /// This gets the number of patients by gender
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{PatientByGender}"/> object.</returns>
        Task<ServiceResponse<ReadPatientByGenderDto>> PatientByGenderAsync(int userId);
        ServiceResponse<object> AvaliableStaff(int clinicId);
        ServiceResponse<object> AllOutPatientAndInPatientCount();

    }
}
