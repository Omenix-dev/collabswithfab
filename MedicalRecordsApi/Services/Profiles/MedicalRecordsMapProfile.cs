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
			CreateMap<UserDto, Employee>().ReverseMap();
			CreateMap<ReadPatientDto, Patient>().ReverseMap();
            CreateMap<ReadVisitHistoryDto, Visit>().ReverseMap();
            CreateMap<CreateLabReferDto, LabRequest>().ReverseMap();
            CreateMap<CreateCustomerFeedbackDto, CustomerFeedback>().ReverseMap();
            CreateMap<Contact, ReadContactDetailsDto>();
            CreateMap<EmergencyContact, ReadEmergencyContactDetailsDto>();
            CreateMap<Immunization, ReadImmunizationRecordDto>();
            CreateMap<ImmunizationDocument, ReadImmunizationDocumentDto>();
            CreateMap<Treatment, ReadTreatmentRecordDto>();
            CreateMap<PatientLabReport, ReadPatientLabReport>();
        }
    }
}
