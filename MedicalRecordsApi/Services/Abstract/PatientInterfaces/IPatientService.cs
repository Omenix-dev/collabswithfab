using MedicalRecordsApi.Models;
using MedicalRecordsApi.Utils;
using MedicalRecordsRepository.DTO.AuthDTO;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.PatientInterfaces
{
    public interface IPatientService
    {
        Task<APIResponse> CreatePatientProfile(CreatePatientProfileDto profileDto ,int userId);
    }
}
