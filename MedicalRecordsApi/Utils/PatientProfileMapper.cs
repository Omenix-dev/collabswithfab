using AutoMapper;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.DTO.MedicalDto;
using MedicalRecordsRepository.DTO.PatientDto;

namespace MedicalRecordsApi.Utils
{
    public class PatientProfileMapper : Profile
    {
        public PatientProfileMapper()
        {
            CreateMap<CreatePatientProfileDto, User>();
            CreateMap<CreatePatientRequestDto, Patient>();
            CreateMap<updateContactDto, Contact>();
            CreateMap<UpdateEmergencyContactDto, EmergencyContact>();
            CreateMap<MedicalRecordsDto, MedicalRecord>();
            CreateMap<MedicalRecord, MedicalRecordsDto>()
                .ForMember(x => x.RecordId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Immunization, ImmunizationDto>()
                .ForMember(x => x.ImmunizationId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ImmunizationDto, Immunization>();
            CreateMap<Visit, PatientsVisitsDto>()
                .ForMember(x => x.VisitId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PatientsVisitsDto, Visit>();
        }
    }
}
