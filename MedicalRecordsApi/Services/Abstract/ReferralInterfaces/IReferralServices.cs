using MedicalRecordsRepository.DTO.ReferralDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.ReferralInterfaces
{
    public interface IReferralServices
    {
        Task<ServiceResponse<string>> AddReferralNote(ReferralNoteDto Note, int UserId);
        Task<ServiceResponse<string>> DeleteReferralNote(int Id);
        Task<ServiceResponse<IEnumerable<GetPatientReferralDto>>> GetAllReferral();
        Task<ServiceResponse<GetPatientReferralDto>> GetAllReferralByPatientId(int patientId);
        Task<ServiceResponse<string>> UpdateReferralNote(ReferralNoteDto Note, int UserId);
    }
}
