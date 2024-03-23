using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MedicalRecordsRepository.DTO.PatientDto;
using MedicalRecordsRepository.DTO.MedicalDto;
using AutoMapper.QueryableExtensions;
using MedicalRecordsApi.Services.Common;
using MedicalRecordsData.Enum;
using System.Linq.Expressions;
using MedicalRecordsRepository.DTO;
using System.Reflection;
using System.Security.Policy;

namespace MedicalRecordsApi.Services.Implementation.PatientServices
{
   

    public class PatientService : IPatientService
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
        private readonly IGenericRepository<Treatment> _treatmentRepository;
        private readonly IGenericRepository<PatientLabReport> _patientLabReportRepository;
        private readonly IGenericRepository<Visit> _visitRepository;
        private readonly IGenericRepository<LabRequest> _labRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<PatientAssignmentHistory> _patientAssignmentHistoryRepository;



        public PatientService(IGenericRepository<Patient> patientRepository,
            IMapper mapper, IConfiguration configuration, IGenericRepository<Contact> contactRepository,
            IGenericRepository<EmergencyContact> emrgencyContactRepository,
            IGenericRepository<Immunization> immunizationRepository,
            IGenericRepository<ImmunizationDocument> immunizationDocumentRepository,
            IGenericRepository<MedicalRecord> medicalRecordRepository,
            IGenericRepository<Medication> medicationRepository,
            IGenericRepository<PatientReferrer> patientReferrerRepository,
            IGenericRepository<Treatment> treatmentRepository, IGenericRepository<Visit> visitRepository,
            MedicalRecordDbContext dbContext, IGenericRepository<Employee> employeeRepository,
            IGenericRepository<LabRequest> labRepository, IGenericRepository<User> userRepository, 
            IGenericRepository<PatientAssignmentHistory> patientAssignmentHistoryRepository, IGenericRepository<PatientLabReport> patientLabReportRepository)
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
            _patientLabReportRepository = patientLabReportRepository;
        }
        #endregion

        public async Task<ServiceResponse<string>> AddPrescriptionAsync(int patientId, int visitId, CreatePatientPrescriptionDto prescriptionDto)
		{
			if (prescriptionDto == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
											.Include(x => x.Visits)
                                            .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResponse<string>("Patient not found", InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            // Filter the visits for the specified visitId
            var visit = patient.Visits.FirstOrDefault(v => v.Id == visitId);

            if (visit == null)
            {
                return new ServiceResponse<string>("Visit not found", InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            Treatment treatment = new Treatment()
			{
				DateOfVisit = prescriptionDto.DateOfVisit,
				Diagnosis = prescriptionDto.Diagnosis,
				Temperature = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Temperature,
				Age = CalculateAge(patient.DateOfBirth),
				Weight = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Weight,
				PatientId = patientId,
				Medications = prescriptionDto.Medication.Select(med => new Medication { Name = med }).ToList()
			};

            patient.Treatments.Add(treatment);

            await _patientRepository.SaveChangesToDbAsync();

            return new ServiceResponse<string>("Successful", InternalCode.Success);
        }

		public async Task<ServiceResponse<string>> AddToPatientNoteAsync(int patientId, CreatePatientNoteDto patientNoteDto)
		{
			if (patientNoteDto == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Treatments.FirstOrDefault(x => x.Id == patientNoteDto.TreatmentId))
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			patient.Treatments.First().AdditonalNote = patientNoteDto.AdditonalNoteOnTreatment;

			await _patientRepository.SaveChangesToDbAsync();

			return new ServiceResponse<string>("Successful", InternalCode.Success);
		}

		public async Task<ServiceResponse<IEnumerable<AssignedPatientsDto>>> GetAssignedPatientsAsync(int userId)
		{
			if (userId <= 0)
			{
				return new ServiceResponse<IEnumerable<AssignedPatientsDto>>(Enumerable.Empty<AssignedPatientsDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

            var patients = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Include(x => x.Visits)
                                                  .Where(x => x.DoctorId == userId).ToListAsync();

            if (!patients.Any())
            {
                return new ServiceResponse<IEnumerable<AssignedPatientsDto>>(Enumerable.Empty<AssignedPatientsDto>(), InternalCode.Success, ServiceErrorMessages.Success);
            }

            List<AssignedPatientsDto> assignedPatientsDTOs = new List<AssignedPatientsDto>();

            foreach (var patient in patients)
            {
                Visit patientVisit = null;

                if (patient.Visits.Any())
                {
                    patientVisit = patient.Visits.OrderBy(x => x.DateOfVisit).Last();
                }
                AssignedPatientsDto assignedPatient = new AssignedPatientsDto()
                {
                    PatientId = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    AssignedNurse = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patient.NurseId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync(),
                    Age = CalculateAge(patient.DateOfBirth),
                    DateCreated = patient.CreatedAt.ToString("dd MMMM yyyy"),
                    Weight = patientVisit?.Weight ?? 0,
                    Height = patientVisit?.Height ?? 0,
                    Temperature = patientVisit?.Temperature ?? 0,
                    Heart = patientVisit?.HeartPulse ?? 0,
                    Resp = patientVisit?.Respiratory.ToString() ?? "0"
                };

                assignedPatientsDTOs.Add(assignedPatient);
            }

            return new ServiceResponse<IEnumerable<AssignedPatientsDto>>(assignedPatientsDTOs, InternalCode.Success);
        }

        public async Task<ServiceResponse<ReadPatientDto>> GetPatientDataAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<ReadPatientDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patient = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Include(x => x.Contact).Include(x => x.EmergencyContact)
                                                  .Include(x => x.Immunizations).Include(x => x.MedicalRecords)
                                                  .Include(x => x.Visits).Include(x => x.Treatments)
                                                  .Include(x => x.PatientReferrer)
                                                    .ProjectTo<ReadPatientDto>(_mapper.ConfigurationProvider)
                                                  .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResponse<ReadPatientDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            //var patientdata = _mapper.Map<ReadPatientDto>(patient);

			patient.NurseName = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patient.NurseId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync();
			patient.DoctorName = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patient.DoctorId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync();

			return new ServiceResponse<ReadPatientDto>(patient, InternalCode.Success);
        }

		public async Task<ServiceResponse<IEnumerable<ReadVisitHistoryDto>>> GetAllAdmissionHistoryAsync(int patientId)
		{
			if (patientId <= 0)
			{
				return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(Enumerable.Empty<ReadVisitHistoryDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Visits)
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(Enumerable.Empty<ReadVisitHistoryDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			var visitsdata = _mapper.Map<IEnumerable<ReadVisitHistoryDto>>(patient.Visits);

			// Update Age for each visit
			visitsdata.ToList().ForEach(visit => visit.Age = CalculateAge(patient.DateOfBirth, visit.DateOfVisit));

			//Update with doctors and nurses names
			foreach (var visit in visitsdata)
			{
				visit.DoctorName = _employeeRepository.Query()
													  .AsNoTracking()
													  .Where(x => x.Id == visit.DoctorId)
													  .Select(s => $"{s.FirstName} {s.LastName}")
													  .FirstOrDefaultAsync()
													  .Result;

				visit.NurseName = _employeeRepository.Query()
													  .AsNoTracking()
													  .Where(x => x.Id == visit.NurseId)
													  .Select(s => $"{s.FirstName} {s.LastName}")
													  .FirstOrDefaultAsync()
													  .Result;
			}

			return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(visitsdata, InternalCode.Success);
        }

		public async Task<ServiceResponse<ReadNurseNotesDto>> GetNurseNoteAsync(int patientId, int visitId)
		{
			if (patientId <= 0 || visitId <= 0)
			{
				return new ServiceResponse<ReadNurseNotesDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Visits.FirstOrDefault(x => x.Id == visitId))
												  .ThenInclude(visit => visit.NurseNotes)
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<ReadNurseNotesDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			var visitsdata = _mapper.Map<IEnumerable<ReadVisitHistoryDto>>(patient.Visits);

			ReadNurseNotesDto readNurseNotesDto = new ReadNurseNotesDto
			{
				Visit = visitsdata.First(),
				Notes = patient.Visits.First().NurseNotes
									  .Select(notes => new NurseNotesDto { Note = notes.Note })
									  .ToList()
			};

			return new ServiceResponse<ReadNurseNotesDto>(readNurseNotesDto, InternalCode.Success);
		}

        public async Task<ServiceResponse<ReadPatientLabReport>> GetLabReportAsync(int patientId, int labrequestId)
        {
            if (patientId <= 0 || labrequestId <= 0)
            {
                return new ServiceResponse<ReadPatientLabReport>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var lab = await _patientLabReportRepository.Query()
										  .AsNoTracking()
										  .Where(lab => lab.PatientId == patientId && lab.LabRequestId == labrequestId)
                                          .Include(x => x.PatientLabDocuments)
                                          .ProjectTo<ReadPatientLabReport>(_mapper.ConfigurationProvider)
                                          .FirstOrDefaultAsync();

			if (lab == null)
			{
				return new ServiceResponse<ReadPatientLabReport>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

            return new ServiceResponse<ReadPatientLabReport>(lab, InternalCode.Success);
        }

        public async Task<ServiceResponse<string>> ReferPatientAsync(int patientId, int visitId, CreateLabReferDto labReferDto)
        {
            if (labReferDto == null || patientId <= 0 || visitId <= 0)
            {
                return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patient = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Include(x => x.Visits.FirstOrDefault(x => x.Id == visitId))
                                                  .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

			var lab = _mapper.Map<LabRequest>(labReferDto);

			patient.Visits.First().Labs.Add(lab);

            await _patientRepository.SaveChangesToDbAsync();

            return new ServiceResponse<string>("Successful", InternalCode.Success);
        }

        public async Task<ServiceResponse<ReadContactDetailsDto>> GetContactDetailsAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<ReadContactDetailsDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var contactDetails = await _contactRepository.Query()
                                          .AsNoTracking()
                                          .Where(contact => contact.PatientId == patientId)
                                          .ProjectTo<ReadContactDetailsDto>(_mapper.ConfigurationProvider)
                                          .FirstOrDefaultAsync();

            if (contactDetails == null)
            {
                return new ServiceResponse<ReadContactDetailsDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<ReadContactDetailsDto>(contactDetails, InternalCode.Success);
        }

        public async Task<ServiceResponse<ReadEmergencyContactDetailsDto>> GetEmergencyContactDetailsAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<ReadEmergencyContactDetailsDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var emergencyContactDetails = await _emergencyContactRepository.Query()
                                                          .AsNoTracking()
                                                          .Where(contact => contact.PatientId == patientId)
                                                          .ProjectTo<ReadEmergencyContactDetailsDto>(_mapper.ConfigurationProvider)
                                                          .FirstOrDefaultAsync();

            if (emergencyContactDetails == null)
            {
                return new ServiceResponse<ReadEmergencyContactDetailsDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<ReadEmergencyContactDetailsDto>(emergencyContactDetails, InternalCode.Success);
        }

        public ServiceResponse<IEnumerable<ReadMedicalRecordDto>> GetMedicalRecordAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadMedicalRecordDto>>(Enumerable.Empty<ReadMedicalRecordDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            IEnumerable<ReadMedicalRecordDto> medicalRecords = _medicalRecordRepository.Query()
                                                                    .AsNoTracking()
                                                                    .Where(record => record.PatientId == patientId)
                                                                    .OrderByDescending(order => order.CreatedAt)
                                                                    .ProjectTo<ReadMedicalRecordDto>(_mapper.ConfigurationProvider);

            if (medicalRecords == null)
            {
                return new ServiceResponse<IEnumerable<ReadMedicalRecordDto>>(Enumerable.Empty<ReadMedicalRecordDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<IEnumerable<ReadMedicalRecordDto>>(medicalRecords, InternalCode.Success);
        }

        public async Task<ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>> GetImmunizationRecordAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>(Enumerable.Empty<ReadImmunizationRecordDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientDOB = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.Id == patientId)
                                                  .Select(x => x.DateOfBirth)
                                                  .FirstOrDefaultAsync();

            if (patientDOB == null)
            {
                return new ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>(Enumerable.Empty<ReadImmunizationRecordDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            IEnumerable<ReadImmunizationRecordDto> immunizationRecords = _immunizationRepository.Query()
                                                                    .AsNoTracking()
                                                                    .Where(record => record.PatientId == patientId)
                                                                    .Include(record => record.ImmunizationDocuments)
                                                                    .OrderByDescending(order => order.CreatedAt)
                                                                    .ProjectTo<ReadImmunizationRecordDto>(_mapper.ConfigurationProvider);

            if (immunizationRecords == null)
            {
                return new ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>(Enumerable.Empty<ReadImmunizationRecordDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            // Update Age for each visit
            immunizationRecords.ToList().ForEach(immunization => immunization.Age = CalculateAge(patientDOB, immunization.DateGiven));

            return new ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>(immunizationRecords, InternalCode.Success);
        }

        public async Task<ServiceResponse<IEnumerable<ReadVisitHistoryDto>>> GetVisitRecordAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(Enumerable.Empty<ReadVisitHistoryDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientDOB = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.Id == patientId)
                                                  .Select(x => x.DateOfBirth)
                                                  .FirstOrDefaultAsync();

            if (patientDOB == null)
            {
                return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(Enumerable.Empty<ReadVisitHistoryDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            IEnumerable<ReadVisitHistoryDto> visitRecords = _visitRepository.Query()
                                                                    .AsNoTracking()
                                                                    .Where(record => record.PatientId == patientId)
                                                                    .OrderByDescending(order => order.CreatedAt)
                                                                    .ProjectTo<ReadVisitHistoryDto>(_mapper.ConfigurationProvider);

            if (visitRecords == null)
            {
                return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(Enumerable.Empty<ReadVisitHistoryDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            // Update Age for each visit
            visitRecords.ToList().ForEach(visit => visit.Age = CalculateAge(patientDOB, visit.DateOfVisit));

            //Update with doctors and nurses names
            foreach (var visit in visitRecords)
            {
                visit.DoctorName = _employeeRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.Id == visit.DoctorId)
                                                      .Select(s => $"{s.FirstName} {s.LastName}")
                                                      .FirstOrDefaultAsync()
                                                      .Result;

                visit.NurseName = _employeeRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.Id == visit.NurseId)
                                                      .Select(s => $"{s.FirstName} {s.LastName}")
                                                      .FirstOrDefaultAsync()
                                                      .Result;
            }

            return new ServiceResponse<IEnumerable<ReadVisitHistoryDto>>(visitRecords, InternalCode.Success);
        }

        public async Task<ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>> GetTreatmentRecordAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>(Enumerable.Empty<ReadTreatmentRecordDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientDOB = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.Id == patientId)
                                                  .Select(x => x.DateOfBirth)
                                                  .FirstOrDefaultAsync();

            if (patientDOB == null)
            {
                return new ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>(Enumerable.Empty<ReadTreatmentRecordDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            IEnumerable<ReadTreatmentRecordDto> treatmentRecords = _visitRepository.Query()
                                                                    .AsNoTracking()
                                                                    .Where(record => record.PatientId == patientId)
                                                                    .OrderByDescending(order => order.CreatedAt)
                                                                    .ProjectTo<ReadTreatmentRecordDto>(_mapper.ConfigurationProvider);

            if (treatmentRecords == null)
            {
                return new ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>(Enumerable.Empty<ReadTreatmentRecordDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            // Update Age for each visit
            treatmentRecords.ToList().ForEach(visit => visit.Age = CalculateAge(patientDOB, visit.DateOfVisit));

            return new ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>(treatmentRecords, InternalCode.Success);
        }

        public async Task<ServiceResponse<string>> CreatePatientProfile(CreatePatientProfileDto profileDto, int userId)
        {
            var userExists = await _userRepository.FirstOrDefault(r => r.Email == profileDto.Email);
            if (userExists != null)
            {
                return new ServiceResponse<string>("the patient already exist", InternalCode.EntityExist, ServiceErrorMessages.EntityExist);

            }
            var userDetails = _mapper.Map<User>(profileDto);
            AuthUtil.CreatePasswordHash(profileDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userDetails.CreatedAt = DateTime.UtcNow;
            userDetails.PasswordHash = passwordHash;
            userDetails.PasswordSalt = passwordSalt;
            userDetails.Email = profileDto.Email;
            userDetails.CreatedBy = userId;

            await _userRepository.Insert(userDetails);
            return new ServiceResponse<string>("the patient profile was created", InternalCode.Success);

        }

        public async Task<ServiceResponse<object>> AddPatient(CreatePatientRequestDto patientDto, int userId)
        {
            try
            {
                if (patientDto.DateOfBirth > DateTime.Now)
                {
                    return new ServiceResponse<object>("The invalid date of birth ", InternalCode.Unprocessable, "The invalid date of birth");

                }
                if (patientDto.PhoneNumber.Length != 11)
                {
                    return new ServiceResponse<object>("The invalid Phone number ", InternalCode.Unprocessable, "The invalid Phone number ");

                }
                var EmailExist = _patientRepository.GetAll().FirstOrDefault(x => x.Email == patientDto.Email);
                if (EmailExist != null)
                {
                    return new ServiceResponse<object>("The patient already exist", InternalCode.EntityExist, ServiceErrorMessages.EntityExist);
                }
                
                var patientDetails = _mapper.Map<Patient>(patientDto);
                patientDetails.CreatedAt = DateTime.UtcNow;
                patientDetails.CreatedBy = userId;
                patientDetails.UserId = userId;
                await _patientRepository.Insert(patientDetails);
                var PatientId = _patientRepository.GetAll().FirstOrDefault(x => x.Email == patientDto.Email).Id;
                return new ServiceResponse<object>(new { Messages = "The patient has been created successfully", PatientId = PatientId }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<object>> AddMedicalReport(MedicalRecordsDto medicalRecords, int userId)
        {
            try
            {
                var patientExist = _patientRepository.GetById(medicalRecords.PatientId.Value);
                if(patientExist == null)
                    return new ServiceResponse<object>("Patient doesnt exist, insert patient", InternalCode.EntityExist, "Patient doesnt exist, insert patient");

                var patientDetails = _mapper.Map<MedicalRecord>(medicalRecords);
                patientDetails.CreatedAt = DateTime.UtcNow;
                patientDetails.CreatedBy = userId;

                await _medicalRecordRepository.Insert(patientDetails);
                var recordId = _medicalRecordRepository.GetAll().FirstOrDefault(x => x.Comment == medicalRecords.Comment && x.Name == medicalRecords.Name).Id;
                return new ServiceResponse<object>(new { Message = "The medical record was added successfully" ,RecordId =recordId }, InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, "Unable to add the medical report");
            }

        }

        public async Task<ServiceResponse<string>> DeleteMedicalReport(int recordId)
        {
            try
            {
                var isExisting = _medicalRecordRepository.GetById(recordId);
                if (isExisting == null)
                    return new ServiceResponse<string>("the medical record doesnt exist", InternalCode.Unprocessable, "the medical record doesnt exist");

                await _medicalRecordRepository.DeleteAsync(recordId);
                return new ServiceResponse<string>("the medical record was deleted successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<List<ResponseMedicalRecordsDto>>> GetAllMedicalReportByPatientId(int patientId)
        {
            try
            {
                var allRecords = await _medicalRecordRepository.GetAll().Where(x => x.PatientId == patientId).ToListAsync();
                var reports = _mapper.Map<List<ResponseMedicalRecordsDto>>(allRecords);
                return new ServiceResponse<List<ResponseMedicalRecordsDto>>(reports, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<ResponseMedicalRecordsDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<object>> AddImmunizationRecords(ImmunizationDto immunizationRecords, int userId)
        {
            try
            {
                var immunizationObj = _mapper.Map<Immunization>(immunizationRecords);
                immunizationObj.CreatedAt = DateTime.UtcNow;
                immunizationObj.CreatedBy = userId;
                await _immunizationRepository.Insert(immunizationObj);
                return new ServiceResponse<object>(new { Message = "The Immunization record was added successfully" }, InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, "Unable to add Immunization record");
            }

        }

        public async Task<ServiceResponse<string>> DeleteImmunizationRecord(int recordId)
        {
            try
            {
                var isExisting = _immunizationRepository.GetById(recordId);
                if (isExisting == null)
                    return new ServiceResponse<string>("the Immunization record doesnt exist", InternalCode.Unprocessable, "the Immunization record doesnt exist");

                await _immunizationRepository.DeleteAsync(recordId);
                return new ServiceResponse<string>("The Immunization record was deleted successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, "Unable to complete the delete phase");
            }

        }

        public async Task<ServiceResponse<List<ResponseImmunizationDto>>> GetAllImmunizatiobByPatientId(int patientId)
        {
            try
            {
                var allRecords = await _immunizationRepository.GetAll().Where(x => x.PatientId == patientId).ToListAsync();
                var reports = _mapper.Map<List<ResponseImmunizationDto>>(allRecords);
                return new ServiceResponse<List<ResponseImmunizationDto>>(reports, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<ResponseImmunizationDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<object>> DeleteVisitsRecord(int visitId)
        {
            try
            {
                var isExisting = _visitRepository.GetById(visitId);
                if (isExisting == null)
                    return new ServiceResponse<object>("The visitation record doesnt exist", InternalCode.Unprocessable, "The visitation record doesnt exist");

                await _visitRepository.DeleteAsync(visitId);
                return new ServiceResponse<object>(new { Message = "The visitation record was deleted successfully" }, InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<object>> AddPatientVistsRecords(PatientsVisitsDto patientVisitsObj, int userId)
        {
            try
            {
                var doctorModel = patientVisitsObj.DoctorEmployeeId != null ? _employeeRepository.GetById(patientVisitsObj.DoctorEmployeeId.Value) : null;
                if (doctorModel == null && patientVisitsObj.DoctorEmployeeId != null && doctorModel.RoleId.Value != (int)MedicalRole.Doctors)
                {
                    return new ServiceResponse<object>("The doctor you assigned doesnt exist", InternalCode.EntityNotFound, "The Doctor you assigned doesnt exist");
                }
                var nurseModel = patientVisitsObj.NurseEmployeeId != null ? _employeeRepository.GetById(patientVisitsObj.NurseEmployeeId.Value) : null;
                if (nurseModel == null && patientVisitsObj.NurseEmployeeId != null && nurseModel.RoleId.Value != (int)MedicalRole.Nurse)
                {
                    return new ServiceResponse<object>("The Nurse you assigned doesnt exist", InternalCode.EntityNotFound, "The Nurse you assigned doesnt exist");

                }
                var visitRecordObj = _mapper.Map<Visit>(patientVisitsObj);
                visitRecordObj.CreatedAt = DateTime.UtcNow;
                visitRecordObj.CreatedBy = userId; 
                await _visitRepository.Insert(visitRecordObj);
                var patientAssignmentHistory = new PatientAssignmentHistory();
                patientAssignmentHistory.CareType = MedicalRecordsData.Enum.PatientCareType.InPatient;
                patientAssignmentHistory.PatientId = patientVisitsObj.PatientId.Value;
                patientAssignmentHistory.NurseId  = patientVisitsObj.NurseEmployeeId;
                patientAssignmentHistory.DoctorId  = patientVisitsObj.DoctorEmployeeId;
                patientAssignmentHistory.CreatedAt = DateTime.UtcNow;
                patientAssignmentHistory.CreatedBy = userId;
                await _patientAssignmentHistoryRepository.CreateAsync(patientAssignmentHistory);
                return new ServiceResponse<object>(new { Message = "The Visit record was added successfully"}, InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<List<ResponsePatientsVisitsDto>>> GetAllVisitationByPatientId(int PatientId)
        {
            try
            {
                var allVisitsRecords = await _visitRepository.GetAll().Where(x => x.PatientId == PatientId).ToListAsync();
                var patientVisits = _mapper.Map<List<ResponsePatientsVisitsDto>>(allVisitsRecords);
                return new ServiceResponse<List<ResponsePatientsVisitsDto>>(patientVisits, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<ResponsePatientsVisitsDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<object>> UpdateContact(UpdateContactDto contactDto, int userId)
        {
            try
            {
                var apiResponse = new ApiResponse();
                var PatientExist = _patientRepository.GetById(contactDto.PatientId);
                if (PatientExist == null)
                {
                    return new ServiceResponse<object>("the patient doesnt exist", InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
                }
                var ContactObject = _contactRepository.GetAll().FirstOrDefault(x => x.PatientId == contactDto.PatientId);
                if (ContactObject == null)
                {
                    var contactDetails = _mapper.Map<Contact>(contactDto);
                    contactDetails.CreatedAt = DateTime.UtcNow;
                    contactDetails.CreatedBy = userId;
                    await _contactRepository.Insert(contactDetails); 
                }
                else
                {
                    ContactObject.StateOfResidence = contactDto.StateOfResidence;
                    ContactObject.LgaResidence  = contactDto.LgaResidence;
                    ContactObject.City  = contactDto.City;
                    ContactObject.HomeAddress  = contactDto.HomeAddress;
                    ContactObject.Phone  = contactDto.Phone;
                    ContactObject.AltPhone  = contactDto.AltPhone;
                    ContactObject.Email  = contactDto.Email;
                    ContactObject.ActionTaken = "Updated contact address";
                    ContactObject.UpdatedAt = DateTime.UtcNow;
                    ContactObject.ModifiedBy = userId;
                    await _contactRepository.UpdateAsync(ContactObject);
                }
                return new ServiceResponse<object>(new { Messages = "The patient contact has been updated", PatientId = contactDto.PatientId }, InternalCode.Success);

            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, "Unable to update contact");
            }

        }

        public async Task<ServiceResponse<object>> UpdateEmergencyContact(UpdateEmergencyContactDto emergencyContactDto, int userId)
        {
            try
            {
                var apiResponse = new ApiResponse();
                var PatientExist = _patientRepository.GetById(emergencyContactDto.PatientId);
                if (PatientExist == null)
                {
                    return new ServiceResponse<object>("the patient doesnt exist", InternalCode.EntityNotFound, "the patient doesnt exist");
                }
                var emergencyObject = _emergencyContactRepository.GetAll().FirstOrDefault(x => x.PatientId == emergencyContactDto.PatientId);
                if (emergencyObject == null)
                {
                    var emergencyDetails = _mapper.Map<EmergencyContact>(emergencyContactDto);
                    emergencyDetails.CreatedAt = DateTime.UtcNow;
                    emergencyDetails.CreatedBy = userId;
                    await _emergencyContactRepository.Insert(emergencyDetails);
                }
                else
                {
                    emergencyObject.Relationship = emergencyContactDto.Relationship;
                    emergencyObject.FirstName  = emergencyContactDto.FirstName;
                    emergencyObject.LastName  = emergencyContactDto.LastName;
                    emergencyObject.Phone  = emergencyContactDto.Phone;
                    emergencyObject.Email  = emergencyContactDto.Email;
                    emergencyObject.ContactAddress  = emergencyContactDto.ContactAddress;
                    emergencyObject.StateOfResidence = emergencyContactDto.StateOfResidence;
                    emergencyObject.Lga  = emergencyContactDto.Lga;
                    emergencyObject.City  = emergencyContactDto.City;
                    emergencyObject.AltPhone  = emergencyContactDto.AltPhone;
                    emergencyObject.ModifiedBy = userId;
                    emergencyObject.UpdatedAt = DateTime.UtcNow;
                    await _emergencyContactRepository.Update(emergencyObject);
                }
                return new ServiceResponse<object>(new { Messages = "The patient emergency contact has been updated", PatientId = emergencyContactDto.PatientId }, InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }
        public async Task<ServiceResponse<object>> UpdateMedicalStaffByPatientId(UpdateMedicalStaffDto updateMedicalStaffDto, int userId)
        {
            try
            {
                var patientObj = _patientRepository.GetById(updateMedicalStaffDto.PatientId.Value);
                if(patientObj == null)
                    return new ServiceResponse<object>("the patientId doesnt exist", InternalCode.EntityIsNull, ServiceErrorMessages.Failed);
                var doctorModel = updateMedicalStaffDto.DoctorEmployeeId != null ? _employeeRepository.GetById(updateMedicalStaffDto.DoctorEmployeeId.Value) : null;
                if (doctorModel == null && updateMedicalStaffDto.DoctorEmployeeId != null&& doctorModel.RoleId.Value != (int)MedicalRole.Doctors)
                {
                    return new ServiceResponse<object>("The doctor you assigned doesnt exist", InternalCode.EntityNotFound, "The Doctor you assigned doesnt exist");
                }
                var nurseModel = updateMedicalStaffDto.NurseEmployeeId != null ? _employeeRepository.GetById(updateMedicalStaffDto.NurseEmployeeId.Value) : null;
                if (nurseModel == null && updateMedicalStaffDto.NurseEmployeeId != null && nurseModel.RoleId.Value != (int)MedicalRole.Nurse)
                {
                    return new ServiceResponse<object>("The Nurse you assigned doesnt exist", InternalCode.EntityNotFound, "The Nurse you assigned doesnt exist");

                }
                patientObj.NurseId = updateMedicalStaffDto.NurseEmployeeId;
                patientObj.DoctorId = updateMedicalStaffDto.DoctorEmployeeId;
                patientObj.ModifiedBy = userId;
                patientObj.UpdatedAt = DateTime.UtcNow;
                await _patientRepository.UpdateAsync(patientObj);
                var patientAssignmentHistory = _mapper.Map<PatientAssignmentHistory>(updateMedicalStaffDto);
                patientAssignmentHistory.CareType = MedicalRecordsData.Enum.PatientCareType.InPatient;
                patientAssignmentHistory.PatientId = updateMedicalStaffDto.PatientId.Value;
                patientAssignmentHistory.CreatedAt = DateTime.UtcNow;   
                patientAssignmentHistory.CreatedBy = userId;
                await _patientAssignmentHistoryRepository.CreateAsync(patientAssignmentHistory);
                return new ServiceResponse<object>(new { Message = "the patient has been reassign" }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }
        public ServiceResponse<PaginatedList<GetAllPatientsDto>> GetAllPatient(int pageIndex,int pageSize)
        {
            try
            {
                var patientsData = _patientRepository.GetAll().AsEnumerable().Select(x => _mapper.Map<GetAllPatientsDto>(x)).AsQueryable();
                var valObject =  new GenericService<GetAllPatientsDto>().SortPaginateByText(pageIndex, pageSize, patientsData, x => x.PatientId.ToString(), Order.Asc);
                return new ServiceResponse<PaginatedList<GetAllPatientsDto>>(valObject, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PaginatedList<GetAllPatientsDto>>(null, InternalCode.Incompleted, ex.Message);
            }
        }
        public ServiceResponse<PaginatedList<GetAllNurseDto>> GetAllNurses(int pageIndex, int pageSize)
        {
            try
            {
                var uservalue = _userRepository.GetAll();
                var allEmployee = _employeeRepository.GetAll();
                var AllNurseDto = (from a in uservalue
                                     join b in allEmployee on a.Id equals b.UserId
                                     select new GetAllNurseDto
                                     {
                                        NurseEmployeeId = b.Id,
                                        Email = a.Email,
                                        Username=a.Username,
                                        RoleId=a.RoleId,
                                        StaffId=a.StaffId,
                                        EmployeeId=b.Id
                                    }).Where(x =>x.RoleId == (int)MedicalRole.Nurse);
                var valObject = new GenericService<GetAllNurseDto>().SortPaginateByText(pageIndex, pageSize, AllNurseDto, x => x.NurseEmployeeId.ToString(), Order.Asc);
                return new ServiceResponse<PaginatedList<GetAllNurseDto>>(valObject, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PaginatedList<GetAllNurseDto>>(null, InternalCode.Incompleted, ex.Message);
            }
        }
        public ServiceResponse<GetAllPatientsDto> GetAllPatientById(int patientId)
        {
            try
            {
                var patientsData = _mapper.Map<GetAllPatientsDto>(_patientRepository.GetAll().FirstOrDefault(x => x.Id == patientId));
                if(patientsData is null)
                    return new ServiceResponse<GetAllPatientsDto>(null, InternalCode.EntityNotFound, "The patient doenst exist");

                return new ServiceResponse<GetAllPatientsDto>(patientsData, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAllPatientsDto>(null, InternalCode.Incompleted, ex.Message);
            }
        }

        public async Task<ServiceResponse<object>> UpdatePatient(UpdatePatientDto updatePatientDto, int userId)
        {
            if (updatePatientDto.PhoneNumber.Length != 11)
            {
                return new ServiceResponse<object>("The invalid Phone number ", InternalCode.Unprocessable, "The invalid Phone number ");

            }
            var EmailExist = _patientRepository.GetAll().FirstOrDefault(x => x.Email == updatePatientDto.Email);
            if (EmailExist == null)
            {
                return new ServiceResponse<object>("The patient doesnt exist", InternalCode.EntityExist, "The patient doesnt exist");
            }
            EmailExist.FirstName = string.IsNullOrEmpty(updatePatientDto.FirstName) ? EmailExist.FirstName : updatePatientDto.FirstName;
            EmailExist.LastName = string.IsNullOrEmpty(updatePatientDto.LastName) ? EmailExist.LastName:updatePatientDto.LastName;
            EmailExist.Gender = string.IsNullOrEmpty(updatePatientDto.Gender) ? EmailExist.Gender:updatePatientDto.Gender;
            EmailExist.PhoneNumber = string.IsNullOrEmpty(updatePatientDto.PhoneNumber) ? EmailExist.PhoneNumber:updatePatientDto.PhoneNumber;
            EmailExist.StateOfOrigin = string.IsNullOrEmpty(updatePatientDto.StateOfOrigin) ? EmailExist.StateOfOrigin:updatePatientDto.StateOfOrigin;
            EmailExist.Lga = string.IsNullOrEmpty(updatePatientDto.Lga) ? EmailExist.Lga:updatePatientDto.Lga;
            EmailExist.PlaceOfBirth = string.IsNullOrEmpty(updatePatientDto.PlaceOfBirth) ? EmailExist.PlaceOfBirth:updatePatientDto.PlaceOfBirth;
            EmailExist.MaritalStatus = string.IsNullOrEmpty(updatePatientDto.MaritalStatus) ? EmailExist.MaritalStatus:updatePatientDto.MaritalStatus;
            EmailExist.Nationality = string.IsNullOrEmpty(updatePatientDto.Nationality) ? EmailExist.Nationality:updatePatientDto.Nationality;
            EmailExist.UpdatedAt = DateTime.UtcNow;
            EmailExist.ModifiedBy = userId;
            await _patientRepository.UpdateAsync(EmailExist);
            return new ServiceResponse<object>(new { Messages = "The patient has details have been updated", PatientId = EmailExist.Id }, InternalCode.Success, ServiceErrorMessages.Success);
        }
        #region Helpers
        public static string CalculateAge(DateTime dateOfBirth, DateTime? currentDate = null)
		{
			// Get the current date
			currentDate ??= DateTime.Now;

			// Calculate the difference in years and months
			int years = currentDate.Value.Year - dateOfBirth.Year;
			int months = currentDate.Value.Month - dateOfBirth.Month;

			// Adjust the age if the birthday has not occurred yet this year
			if (currentDate.Value.Month < dateOfBirth.Month || (currentDate.Value.Month == dateOfBirth.Month && currentDate.Value.Day < dateOfBirth.Day))
			{
				years--;
				months += 12; // Add 12 months to represent the remaining months until the birthday
			}

            // Construct the age string
            string ageString = $"{(years > 0 ? $"{years} {(years == 1 ? "year" : "years")}" : "")}{(years > 0 && months > 0 ? ", " : "")}{(months > 0 ? $"{months} {(months == 1 ? "month" : "months")}" : "")}";

			return ageString;
		}

        
        #endregion
    }
}
