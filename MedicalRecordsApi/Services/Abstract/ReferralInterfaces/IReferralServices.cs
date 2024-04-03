using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
using MedicalRecordsRepository.DTO.ReferralDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.ReferralInterfaces
{
    public interface IReferralServices
    {
        Task<ServiceResponse<object>> AddReferral(ReferralDto Note, int UserId);
        ServiceResponse<PaginatedList<GetPatientReferralDto>> GetAllReferral(int ClinicId, int pageIndex, int pageSize, string search, FilterBy FilterBy);
        ServiceResponse<GetPatientReferralDto> GetAllReferralByReferralId(int ReferralId);
        Task<ServiceResponse<object>> RemoveReferredPatient(int Id);
        Task<ServiceResponse<object>> UpdateReferralNote(ReferralNoteDto Note, int UserId);
    }
}
