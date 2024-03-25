using MedicalRecordsRepository.DTO;
using MedicalRecordsRepository.DTO.ReferralDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.ReferralInterfaces
{
    public interface IReferralServices
    {
        Task<ServiceResponse<string>> AddReferral(ReferralDto Note, int UserId);
        ServiceResponse<PaginatedList<GetPatientReferralDto>> GetAllReferral(int ClinicId, int pageIndex, int pageSize);
        ServiceResponse<GetPatientReferralDto> GetAllReferralByPatientId(int ClinicId, int patientId);
        Task<ServiceResponse<object>> RemoveReferredPatient(int Id);
        Task<ServiceResponse<string>> UpdateReferralNote(ReferralNoteDto Note, int UserId);
    }
}
