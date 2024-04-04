using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsRepository.DTO.FacilityDto;
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
        Task<ServiceResponse<IEnumerable<ReadBedDetailsDto>>> GetBedsAssignedToDoctor(int userId);
        /// <summary>
        /// This gets bed information. If free, If assigned to doctor. Etc
        /// </summary>
        /// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        //Task<ServiceResponse<IEnumerable<ReadBedDetailsDto>>> GetBedsAssignedToNurse(int userId);
        ///// <summary>
        ///// This gets bed information. If free, or occupied
        ///// </summary>
        ///// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadBedDetailsDTO}}"/> object.</returns>
        //Task<ServiceResponse<IEnumerable<ReadBedDetailsDto>>> GetBedStatus();
        ///// <summary>
        ///// assign bed space
        ///// </summary>
        ///// <param name="bedSpaceDto"></param>
        ///// <returns></returns>
        //Task<ServiceResponse<string>> AssignBed(AssignBedRequestDto bedSpaceDto, int userId);
        ///// <summary>
        ///// used to free the bedspace 
        ///// </summary>
        ///// <param name="patientBedSpace"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //Task<ServiceResponse<string>> FreeBedSpace(int patientBedSpace, int userId);
        ServiceResponse<int> GetTotalAvailableBeds();
        ServiceResponse<int> GetTotalOccupiedBeds();
    }
}
