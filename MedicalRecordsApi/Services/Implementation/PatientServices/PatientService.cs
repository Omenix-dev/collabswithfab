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
		private readonly IGenericRepository<EmergencyContact> _emrgencyContactRepository;
		private readonly IGenericRepository<Immunization> _immunizationRepository;
		private readonly IGenericRepository<ImmunizationDocument> _immunizationDocumentRepository;
		private readonly IGenericRepository<MedicalRecord> _medicalRecordRepository;
		private readonly IGenericRepository<Medication> _medicationRepository;
		private readonly IGenericRepository<PatientReferrer> _patientReferrerRepository;
		private readonly IGenericRepository<Treatment> _treatmentRepository;
        private readonly IGenericRepository<Visit> _visitRepository;
        private readonly IGenericRepository<Lab> _labRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<User> _userRepository;

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
            IGenericRepository<Lab> labRepository, IGenericRepository<User> userRepository)
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
            _dbContext = dbContext;
            _employeeRepository = employeeRepository;
            _labRepository = labRepository;
            _userRepository = userRepository;
        }
        #endregion

        public async Task<ServiceResponse<string>> AddPrescriptionAsync(int patientId, CreatePatientPrescriptionDTO prescriptionDTO)
		{
			if (prescriptionDTO == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
											.Include(x => x.Visits)
											.FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

			Treatment treatment = new Treatment()
			{
				DateOfVisit = prescriptionDTO.DateOfVisit,
				Diagnosis = prescriptionDTO.Diagnosis,
				Temperature = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Temperature,
				Age = CalculateAge(patient.DateOfBirth),
				Weight = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Weight,
				PatientId = patientId,
				Medications = prescriptionDTO.Medication.Select(med => new Medication { Name = med }).ToList()
			};

            patient.Treatments.Add(treatment);

            await _patientRepository.SaveChangesToDbAsync();

            return new ServiceResponse<string>("Successful", InternalCode.Success);
        }

		public async Task<ServiceResponse<string>> AddToPatientNoteAsync(int patientId, CreatePatientNoteDTO patientNoteDTO)
		{
			if (patientNoteDTO == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Treatments.FirstOrDefault(x => x.Id == patientNoteDTO.TreatmentId))
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			patient.Treatments.First().AdditonalNote = patientNoteDTO.AdditonalNoteOnTreatment;

			await _patientRepository.SaveChangesToDbAsync();

			return new ServiceResponse<string>("Successful", InternalCode.Success);
		}

		public async Task<ServiceResponse<IEnumerable<AssignedPatientsDTO>>> GetAssignedPatientsAsync(int userId)
		{
			if (userId <= 0)
			{
				return new ServiceResponse<IEnumerable<AssignedPatientsDTO>>(Enumerable.Empty<AssignedPatientsDTO>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

            var patients = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Include(x => x.Visits)
                                                  .Where(x => x.DoctorId == userId).ToListAsync();

            if (!patients.Any())
            {
                return new ServiceResponse<IEnumerable<AssignedPatientsDTO>>(Enumerable.Empty<AssignedPatientsDTO>(), InternalCode.Success, ServiceErrorMessages.Success);
            }

			List<AssignedPatientsDTO> assignedPatientsDTOs = patients.Select(patient => new AssignedPatientsDTO
			{
				PatientId = patient.Id,
				FirstName = patient.FirstName,
				LastName = patient.LastName,
				AssignedNurse = _employeeRepository.Query()
								.AsNoTracking()
								.Where(x => x.Id == patient.NurseId)
								.Select(s => $"{s.FirstName} {s.LastName}")
								.FirstOrDefaultAsync()
								.Result, // Use .Result to get the result synchronously, assuming this is an asynchronous operation
				Age = CalculateAge(patient.DateOfBirth),
				DateCreated = patient.CreatedAt.ToString("dd MMMM yyyy"),
				Weight = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Weight,
				Height = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Height,
				Temperature = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Temperature,
				Heart = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Height,
				Resp = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Respiratory
			}).ToList();
			//List<AssignedPatientsDTO> assignedPatientsDTOs = new List<AssignedPatientsDTO>();

			//foreach (var patient in patients)
			//{
			//	AssignedPatientsDTO assignedPatient = new AssignedPatientsDTO()
			//	{
			//		PatientId = patient.PatientId,
			//		FirstName = patient.FirstName,
			//		LastName = patient.LastName,
			//		AssignedNurse = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patient.NurseId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync(),
			//		Age = CalculateAge(patient.DateOfBirth),
			//		DateCreated = patient.CreatedAt.ToString("dd MMMM yyyy"),
			//		Weight = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Weight,
			//		Height = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Height,
			//		Temperature = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Temperature,
			//		Heart = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Height,
			//		Resp = patient.Visits.OrderBy(x => x.DateOfVisit).Last().Respiratory
			//	};

			//	assignedPatientsDTOs.Add(assignedPatient);
			//}

            return new ServiceResponse<IEnumerable<AssignedPatientsDTO>>(assignedPatientsDTOs, InternalCode.Success);
        }

        public async Task<ServiceResponse<ReadPatientDTO>> GetPatientDataAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new ServiceResponse<ReadPatientDTO>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patient = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Include(x => x.Contact).Include(x => x.EmergencyContact)
                                                  .Include(x => x.Immunizations).Include(x => x.MedicalRecords)
                                                  .Include(x => x.Contact).Include(x => x.Contact)
                                                  .Include(x => x.Contact).Include(x => x.Contact)
                                                  .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResponse<ReadPatientDTO>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            var patientdata = _mapper.Map<ReadPatientDTO>(patient);

			patientdata.NurseName = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patientdata.NurseId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync();
			patientdata.DoctorName = await _employeeRepository.Query().AsNoTracking().Where(x => x.Id == patientdata.DoctorId).Select(s => $"{s.FirstName} {s.LastName}").FirstOrDefaultAsync();

			return new ServiceResponse<ReadPatientDTO>(patientdata, InternalCode.Success);
        }

		public async Task<ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>> GetAllAdmissionHistoryAsync(int patientId)
		{
			if (patientId <= 0)
			{
				return new ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>(Enumerable.Empty<ReadVisitHistoryDTO>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Visits)
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>(Enumerable.Empty<ReadVisitHistoryDTO>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			var visitsdata = _mapper.Map<IEnumerable<ReadVisitHistoryDTO>>(patient.Visits);

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

			return new ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>(visitsdata, InternalCode.Success);
        }

		public async Task<ServiceResponse<ReadNurseNotesDTO>> GetNurseNoteAsync(int patientId, int visitId)
		{
			if (patientId <= 0 || visitId <= 0)
			{
				return new ServiceResponse<ReadNurseNotesDTO>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
			}

			var patient = await _patientRepository.Query()
												  .AsNoTracking()
												  .Include(x => x.Visits.FirstOrDefault(x => x.Id == visitId))
												  .ThenInclude(visit => visit.NurseNotes)
												  .FirstOrDefaultAsync(x => x.Id == patientId);

			if (patient == null)
			{
				return new ServiceResponse<ReadNurseNotesDTO>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

			var visitsdata = _mapper.Map<IEnumerable<ReadVisitHistoryDTO>>(patient.Visits);

			ReadNurseNotesDTO readNurseNotesDTO = new ReadNurseNotesDTO
			{
				Visit = visitsdata.First(),
				Notes = patient.Visits.First().NurseNotes
									  .Select(notes => new NurseNotesDTO { Note = notes.Note })
									  .ToList()
			};

			return new ServiceResponse<ReadNurseNotesDTO>(readNurseNotesDTO, InternalCode.Success);
		}

        public async Task<ServiceResponse<string>> GetLabNoteAsync(int patientId, int labId)
        {
            if (patientId <= 0 || labId <= 0)
            {
                return new ServiceResponse<string>(String.Empty, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var lab = await _labRepository.Query()
										  .AsNoTracking()
										  .Where(lab => lab.Visit.PatientId == patientId && lab.Id == labId)
										  .FirstOrDefaultAsync();

			if (lab == null)
			{
				return new ServiceResponse<string>(String.Empty, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
			}

            return new ServiceResponse<string>(lab.LabNote, InternalCode.Success);
        }

        public async Task<ServiceResponse<string>> ReferPatientAsync(int patientId, int visitId, CreateLabReferDTO labReferDTO)
        {
            if (labReferDTO == null || patientId <= 0 || visitId <= 0)
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

			var lab = _mapper.Map<Lab>(labReferDTO);

			patient.Visits.First().Labs.Add(lab);

            await _patientRepository.SaveChangesToDbAsync();

            return new ServiceResponse<string>("Successful", InternalCode.Success);
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

        public async Task<ServiceResponse<string>> AddPatient(CreatePatientRequestDto patientDto, int userId)
        {
            try
            {
                var PatientDetails = _mapper.Map<Patient>(patientDto);
                PatientDetails.CreatedAt = DateTime.UtcNow;
                PatientDetails.CreatedBy = userId;

                await _patientRepository.Insert(PatientDetails);

                return new ServiceResponse<string>("the patient profile was created", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> AddMedicalReport(MedicalRecordsDto MedicalRecords, int userId)
        {
            try
            {
                var PatientDetails = _mapper.Map<MedicalRecord>(MedicalRecords);
                PatientDetails.CreatedAt = DateTime.UtcNow;
                PatientDetails.CreatedBy = userId;

                await _medicalRecordRepository.Insert(PatientDetails);

                return new ServiceResponse<string>("the medical record was added successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> DeleteMedicalReport(int RecordId)
        {
            try
            {
                await _medicalRecordRepository.DeleteAsync(RecordId);
                return new ServiceResponse<string>("the medical record was deleted successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<List<MedicalRecordsDto>>> GetAllMedicalReportByPatientId(int patientId)
        {
            try
            {
                var allRecords = await _medicalRecordRepository.GetAll().Where(x => x.Id == patientId).ToListAsync();
                var reports = _mapper.Map<List<MedicalRecordsDto>>(allRecords);
                return new ServiceResponse<List<MedicalRecordsDto>>(reports, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<MedicalRecordsDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> AddImmunizationRecords(ImmunizationDto ImmunizationRecords, int userId)
        {
            try
            {
                var immunizationObj = _mapper.Map<Immunization>(ImmunizationRecords);
                immunizationObj.CreatedAt = DateTime.UtcNow;
                immunizationObj.CreatedBy = userId;
                await _immunizationRepository.Insert(immunizationObj);
                return new ServiceResponse<string>("the Immunization record was added successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> DeleteImmunizationRecord(int RecordId)
        {
            try
            {
                await _immunizationRepository.DeleteAsync(RecordId);
                return new ServiceResponse<string>("the Immunization record was deleted successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<List<ImmunizationDto>>> GetAllImmunizatiobByPatientId(int patientId)
        {
            try
            {
                var allRecords = await _immunizationRepository.GetAll().Where(x => x.Id == patientId).ToListAsync();
                var reports = _mapper.Map<List<ImmunizationDto>>(allRecords);
                return new ServiceResponse<List<ImmunizationDto>>(reports, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<ImmunizationDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> DeleteVisitsRecord(int VisitId)
        {
            try
            {
                await _immunizationRepository.DeleteAsync(VisitId);
                return new ServiceResponse<string>("the visitation record was deleted successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> AddPatientVistsRecords(PatientsVisitsDto PatientVisitsObj, int userId)
        {
            try
            {
                var VisitRecordObj = _mapper.Map<Visit>(PatientVisitsObj);
                VisitRecordObj.CreatedAt = DateTime.UtcNow;
                VisitRecordObj.CreatedBy = userId;
                await _visitRepository.Insert(VisitRecordObj);
                return new ServiceResponse<string>("the Visit record was added successfully", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<List<PatientsVisitsDto>>> GetAllVisitationByPatientId(int VisitaionId)
        {
            try
            {
                var allVisitsRecords = await _visitRepository.GetAll().Where(x => x.Id == VisitaionId).ToListAsync();
                var patientVisits = _mapper.Map<List<PatientsVisitsDto>>(allVisitsRecords);
                return new ServiceResponse<List<PatientsVisitsDto>>(patientVisits, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<List<PatientsVisitsDto>>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> UpdateContact(updateContactDto contactDto, int userId)
        {
            try
            {
                var ApiResponse = new APIResponse();
                var contactDetails = _mapper.Map<Contact>(contactDto);
                contactDetails.CreatedAt = DateTime.UtcNow;
                contactDetails.ModifiedBy = userId;
                await _contactRepository.Insert(contactDetails);
                return new ServiceResponse<string>("the patient profile was updated", InternalCode.Success);

            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }

        public async Task<ServiceResponse<string>> UpdateEmergencyContact(UpdateEmergencyContactDto emergencyContactDto, int userId)
        {
            try
            {
                var ApiResponse = new APIResponse();
                var emergencyDetails = _mapper.Map<EmergencyContact>(emergencyContactDto);
                emergencyDetails.CreatedAt = DateTime.UtcNow;
                emergencyDetails.ModifiedBy = userId;
                await _emrgencyContactRepository.Insert(emergencyDetails);
                return new ServiceResponse<string>("the patient emergency co was updated", InternalCode.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
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
