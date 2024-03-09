using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request.Enums;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.DashBoardInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.DashBoardServices
{
    public class DashBoardService : IDashBoardService
    {
        #region config
        private readonly IMapper _mapper;
        private readonly MedicalRecordDbContext _dbContext;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly IGenericRepository<EmergencyContact> _emergencyContactRepository;
        private readonly IGenericRepository<Immunization> _immunizationRepository;
        private readonly IGenericRepository<ImmunizationDocument> _immunizationDocumentRepository;
        private readonly IGenericRepository<MedicalRecord> _medicalRecordRepository;
        private readonly IGenericRepository<Medication> _medicationRepository;
        private readonly IGenericRepository<PatientReferrer> _patientReferrerRepository;
        private readonly IGenericRepository<PatientAssignmentHistory> _patientAssignmentHistoryRepository;
        private readonly IGenericRepository<Treatment> _treatmentRepository;
        private readonly IGenericRepository<Visit> _visitRepository;
        private readonly IGenericRepository<Lab> _labRepository;
        private readonly IGenericRepository<BedAssignment> _bedAssignmentRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<User> _userRepository;

        public DashBoardService(IGenericRepository<Patient> patientRepository,
            IMapper mapper, IConfiguration configuration, IGenericRepository<Contact> contactRepository,
            IGenericRepository<EmergencyContact> emrgencyContactRepository,
            IGenericRepository<Immunization> immunizationRepository,
            IGenericRepository<ImmunizationDocument> immunizationDocumentRepository,
            IGenericRepository<MedicalRecord> medicalRecordRepository,
            IGenericRepository<Medication> medicationRepository,
            IGenericRepository<PatientReferrer> patientReferrerRepository,
            IGenericRepository<Treatment> treatmentRepository, IGenericRepository<Visit> visitRepository,
            MedicalRecordDbContext dbContext, IGenericRepository<Employee> employeeRepository,
            IGenericRepository<Lab> labRepository, IGenericRepository<User> userRepository, IGenericRepository<PatientAssignmentHistory> patientAssignmentHistoryRepository, IGenericRepository<BedAssignment> bedAssignmentRepository)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _configuration = configuration;
            _contactRepository = contactRepository;
            _emergencyContactRepository = emrgencyContactRepository;
            _immunizationRepository = immunizationRepository;
            _immunizationDocumentRepository = immunizationDocumentRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _medicationRepository = medicationRepository;
            _patientReferrerRepository = patientReferrerRepository;
            _treatmentRepository = treatmentRepository;
            _visitRepository = visitRepository;
            _dbContext = dbContext;
            _employeeRepository = employeeRepository;
            _labRepository = labRepository;
            _userRepository = userRepository;
            _patientAssignmentHistoryRepository = patientAssignmentHistoryRepository;
            _bedAssignmentRepository = bedAssignmentRepository;
        }
        #endregion

        public async Task<ServiceResponse<long>> GetAssignedPatientsCountAsync(int userId, PatientCareStatus status)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = 0;

            if (status == PatientCareStatus.All)
            {
                patientCount = await _patientAssignmentHistoryRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId 
                                                       || x.NurseId == userId)
                                                      .Select(x => x.PatientId)
                                                      .Distinct().CountAsync();
            }
            else if (status == PatientCareStatus.Waiting)
            {
                patientCount = await _patientRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId).CountAsync();
            }

            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetAdmittedPatientsCountAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var bedsAssigned = _bedAssignmentRepository.Query()
                                                       .AsNoTracking().ToList();

            if (!bedsAssigned.Any())
            {
                return new ServiceResponse<long>(0, InternalCode.Success);
            }


            var patients = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.DoctorId == userId).ToListAsync();

            if (!patients.Any())
            {
                return new ServiceResponse<long>(0, InternalCode.Success);
            }

            HashSet<int> patientIds = new HashSet<int>(bedsAssigned.Select(x => x.PatientId).ToList());
            IEnumerable<int> patientCount = patients.Select(x => x.Id).ToList().Where(patientIds.Contains);

            return new ServiceResponse<long>(patientCount.Count(), InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetInPatientOutPatientPatientsCountAsync(int userId, PatientCareType careType)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = patientCount = await _patientAssignmentHistoryRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId && x.CareType == careType)
                                                      .Select(x => x.PatientId)
                                                      .Distinct().CountAsync();
            
            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<ReadPatientCareTypeDto>> InPatientOutPatientDataAndPercentagesAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<ReadPatientCareTypeDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            int totalCount = await _patientAssignmentHistoryRepository.Query().AsNoTracking()
                                                            .Where(history => history.DoctorId == userId)
                                                            .CountAsync();

            var careCount = await _patientAssignmentHistoryRepository.Query().AsNoTracking()
                                                                     .Where(history => history.DoctorId == userId)
                                                                     .GroupBy(history => history.CareType)
                                                                     .Select(group => new ReadPatientCareTypeDto
                                                                     {
                                                                         InPatientPercentage = group.Key == PatientCareType.InPatient ? (double)group.Count() / totalCount * 100 : 0,
                                                                         OutPatientPercentage = group.Key == PatientCareType.OutPatient ? (double)group.Count() / totalCount * 100 : 0
                                                                     })
                                                                     .FirstOrDefaultAsync();

            if (careCount == null)
            {
                return new ServiceResponse<ReadPatientCareTypeDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            var dailyAverages = await _patientAssignmentHistoryRepository.Query().AsNoTracking()
                                                                         .Where(history => history.DoctorId == userId)
                                                                         .GroupBy(history => history.CreatedAt)
                                                                         .Select(group => new DailyAverageCount
                                                                         {
                                                                             Date = group.Key.ToString("MMM dd"),
                                                                             InPatientCount = group.Count(x => x.CareType == PatientCareType.InPatient),
                                                                             OutPatientCount = group.Count(x => x.CareType == PatientCareType.OutPatient)
                                                                         })
                                                                         .ToListAsync();
            careCount.DailyAverageCount.AddRange(dailyAverages);  

            return new ServiceResponse<ReadPatientCareTypeDto>(careCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<ReadPatientByGenderDto>> PatientByGenderAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<ReadPatientByGenderDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            int totalCount = await _patientRepository.Query().AsNoTracking()
                                                     .Where(history => history.DoctorId == userId)
                                                     .CountAsync();

            var careCount = await _patientRepository.Query().AsNoTracking()
                                                    .Where(history => history.DoctorId == userId)
                                                    .GroupBy(history => history.Gender)
                                                    .Select(group => new ReadPatientByGenderDto
                                                    {
                                                        MalePatientPercentage = group.Key.ToLower() == "male" ? (double)group.Count() / totalCount * 100 : 0,
                                                        FemalePatientPercentage = group.Key.ToLower() == "female" ? (double)group.Count() / totalCount * 100 : 0
                                                    })
                                                    .FirstOrDefaultAsync();

            if (careCount == null)
            {
                return new ServiceResponse<ReadPatientByGenderDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<ReadPatientByGenderDto>(careCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>> PatientAdmissionAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(Enumerable.Empty<ReadPatientAdmissionDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientAdmission = await _patientRepository.Query().AsNoTracking()
                                                     .Where(history => history.DoctorId == userId)
                                                     .GroupBy(admission => admission.CreatedAt.Hour)
                                                     .Select(group => new ReadPatientAdmissionDto
                                                     {
                                                         Time = $"{group.Key}:00 - {group.Key + 1}:00",
                                                         Count = (long)Math.Round(group.Average(admission => admission.CreatedAt.Hour), MidpointRounding.ToEven)
                                                     })
                                                     .ToListAsync();

            if (!patientAdmission.Any())
            {
                return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(Enumerable.Empty<ReadPatientAdmissionDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(patientAdmission, InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetPatientByHmoAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = patientCount = await _patientRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId && x.HasHmo == true).CountAsync();

            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }
    }
}
