using AutoMapper;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;

namespace MedicalRecordsApi.Services.Profiles
{
    public class MedicalRecordsMapProfile : Profile
    {
        public MedicalRecordsMapProfile()
		{
			CreateMap<UserDTO, Employee>().ReverseMap();
			CreateMap<ReadPatientDTO, Patient>().ReverseMap();
            CreateMap<ReadVisitHistoryDTO, Visit>().ReverseMap();
            CreateMap<CreateLabReferDTO, Lab>().ReverseMap();
        }
    }
}
