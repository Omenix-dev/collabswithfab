using AutoMapper;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.DTO.MedicalDto;
using MedicalRecordsRepository.DTO.PatientDto;
using MedicalRecordsRepository.DTO.ReferralDto;

namespace MedicalRecordsApi.Services.Profiles
{
    public class PatientProfileMapper : Profile
    {
        public PatientProfileMapper()
        {
            CreateMap<CreatePatientProfileDto, User>();
            CreateMap<CreatePatientRequestDto, Patient>();
            CreateMap<Patient, GetAllPatientsDto>()
                .ForMember(x =>x.PatientId, src => src.MapFrom(s => s.Id)); 
            CreateMap<UpdateContactDto, Contact>();
            CreateMap<UpdateEmergencyContactDto, EmergencyContact>();
            CreateMap<MedicalRecordsDto, MedicalRecord>();
            CreateMap<MedicalRecord, MedicalRecordsDto>();
            CreateMap<MedicalRecord, ResponseMedicalRecordsDto>()
                .ForMember(x => x.RecordId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Immunization, ImmunizationDto>();
            CreateMap<Immunization, ResponseImmunizationDto>()
                .ForMember(x => x.ImmunizationId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ImmunizationDto, Immunization>();
            CreateMap<Visit, PatientsVisitsDto>();
            CreateMap<Visit, ResponsePatientsVisitsDto>()
                .ForMember(x => x.VisitId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PatientsVisitsDto, Visit>()
                .ForMember(x => x.NurseId, opt => opt.MapFrom(src => src.NurseEmployeeId))
                .ForMember(x => x.DoctorId, opt => opt.MapFrom(src => src.DoctorEmployeeId));
            //CreateMap<AssignedPatientsDto, BedAssignment>().ReverseMap();
            CreateMap<PatientReferrer, ReferralNoteDto>().ReverseMap();
            CreateMap<CreatePatientRequestDto, PatientAssignmentHistory>();
            CreateMap<UpdateMedicalStaffDto, PatientAssignmentHistory>();
            CreateMap<Patient, AssignedPatientsDto>();
        }
    }
}
