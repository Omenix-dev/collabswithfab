using AutoMapper;
using MedicalRecordsApi.Models;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.PatientServices
{
    public class PatientService : IPatientService
    {
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
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;


        public PatientService(IGenericRepository<Patient> patientRepository,
			IMapper mapper, IConfiguration configuration, IGenericRepository<Contact> contactRepository, 
			IGenericRepository<EmergencyContact> emrgencyContactRepository, 
			IGenericRepository<Immunization> immunizationRepository, 
			IGenericRepository<ImmunizationDocument> immunizationDocumentRepository, 
			IGenericRepository<MedicalRecord> medicalRecordRepository, 
			IGenericRepository<Medication> medicationRepository, 
			IGenericRepository<PatientReferrer> patientReferrerRepository, 
			IGenericRepository<Treatment> treatmentRepository, 
			IGenericRepository<Visit> visitRepository,
            IGenericRepository<User> userRepository
			)
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
            _userRepository = userRepository;
        }

        public async Task<APIResponse> CreatePatientProfile(CreatePatientProfileDto profileDto, int userId)
        {
			var ApiResponse = new APIResponse();

			var userExists = await _userRepository.FirstOrDefault(r => r.Email == profileDto.Email);
			if (userExists != null)
			{
				ApiResponse.StatusCode = "01";
				ApiResponse.ApiMessage = $"user already {profileDto.Email} exist";
				ApiResponse.Result = null;
				return ApiResponse;
			}
			var userDetails = _mapper.Map<User>(profileDto);
			AuthUtil.CreatePasswordHash(profileDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
			userDetails.CreatedAt = DateTime.UtcNow;
            userDetails.PasswordHash = passwordHash;
            userDetails.PasswordSalt = passwordSalt;
            userDetails.Email = profileDto.Email;
			userDetails.CreatedBy = userId;

            await _userRepository.Insert(userDetails);
            ApiResponse.StatusCode = "01";
            ApiResponse.ApiMessage = $"user created successfully";
            ApiResponse.Result = profileDto.Email;

            return ApiResponse;
        }
    }
}
