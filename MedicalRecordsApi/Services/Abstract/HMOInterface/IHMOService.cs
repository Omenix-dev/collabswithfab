using MedicalRecordsApi.Models.DTO.Responses;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.HMOInterface
{
    public interface IHMOService
    {
        Task<ServiceResponse<object>> AddPatientHMOAsync(PatientHMODto patientHMODto, int UserId);
        Task<ServiceResponse<object>> UpdatePatientHMOAsync(UpdatePatientHMODto updatePatientHMODto, int UserId);
    }
}
