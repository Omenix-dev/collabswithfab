using AutoMapper;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsRepository.DTO.AuthDTO;

namespace MedicalRecordsApi.Services.Profiles
{
    public class MedicalRecordsMapProfile : Profile
    {
        public MedicalRecordsMapProfile()
        {
            CreateMap<UserDTO, Employee>().ReverseMap();
        }
    }
}
