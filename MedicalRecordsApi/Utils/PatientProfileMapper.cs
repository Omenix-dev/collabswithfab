using AutoMapper;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsRepository.DTO.AuthDTO;

namespace MedicalRecordsApi.Utils
{
    public class PatientProfileMapper : Profile
    {
        public PatientProfileMapper()
        {
            CreateMap<CreatePatientProfileDto, User>();
        }
    }
}
