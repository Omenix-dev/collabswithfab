using AutoMapper;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.PatientServices
{
    public class PatientService : IPatientService
    {
		#region config
		private readonly IMapper _mapper;
		private readonly IGenericRepository<Patient> _patientRepository;
		private readonly IGenericRepository<Contact> _contactRepository;
		private readonly IGenericRepository<EmergencyContact> _emrgencyContactRepository;
		private readonly IGenericRepository<Immunization> _immunizationRepository;
		private readonly IGenericRepository<ImmunizationDocument> _immunizationDocumentRepository;
		private readonly IGenericRepository<MedicalRecord> _medicalRecordRepository;
		private readonly IGenericRepository<Medication> _medicationRepository;
		private readonly IGenericRepository<PatientReferrer> _patientReferrerRepository;
		private readonly IGenericRepository<Treatment> _treatmentRepository;
		private readonly IGenericRepository<Visit> _visitRepository;
		private readonly IConfiguration _configuration;

		public PatientService(IGenericRepository<Patient> patientRepository,
			IMapper mapper, IConfiguration configuration, IGenericRepository<Contact> contactRepository, 
			IGenericRepository<EmergencyContact> emrgencyContactRepository, 
			IGenericRepository<Immunization> immunizationRepository, 
			IGenericRepository<ImmunizationDocument> immunizationDocumentRepository, 
			IGenericRepository<MedicalRecord> medicalRecordRepository, 
			IGenericRepository<Medication> medicationRepository, 
			IGenericRepository<PatientReferrer> patientReferrerRepository, 
			IGenericRepository<Treatment> treatmentRepository, IGenericRepository<Visit> visitRepository)
		{
			_patientRepository = patientRepository;
			_mapper = mapper;
			_configuration = configuration;
			_contactRepository = contactRepository;
			_emrgencyContactRepository = emrgencyContactRepository;
			_immunizationRepository = immunizationRepository;
			_immunizationDocumentRepository = immunizationDocumentRepository;
			_medicalRecordRepository = medicalRecordRepository;
			_medicationRepository = medicationRepository;
			_patientReferrerRepository = patientReferrerRepository;
			_treatmentRepository = treatmentRepository;
			_visitRepository = visitRepository;
		}
		#endregion



		public Task<ServiceResponse<string>> AddPrescriptionAsync(CreatePatientPrescriptionDTO prescriptionDTO)
		{
			throw new System.NotImplementedException();
		}

		public Task<ServiceResponse<string>> AddToPatientNoteAsync(CreatePatientNoteDTO patientNoteDTO)
		{
			throw new System.NotImplementedException();
		}

		public Task<ServiceResponse<List<AssignedPatientsDTO>>> GetAssignedPatientsAsync(int userId)
		{
			throw new System.NotImplementedException();
		}

		public Task<ServiceResponse<ReadPatientDTO>> GetPatientDataAsync(int patientId)
		{
			throw new System.NotImplementedException();
		}
	}
}
