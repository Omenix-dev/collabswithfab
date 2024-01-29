using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Abstract.FacilityInterfaces
{
    public interface IFacilityService
    {
        /// <summary>
        /// This gets bed information. If free, If assigned to doctor. Etc
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        Task<ServiceResponse<IEnumerable<ReadBedDetailsDTO>>> GetBedsAssignedToDoctor(int userId);
        /// <summary>
        /// This gets bed information. If free, or occupied
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        Task<ServiceResponse<IEnumerable<ReadBedDetailsDTO>>> GetBedStatus();
    }
}
