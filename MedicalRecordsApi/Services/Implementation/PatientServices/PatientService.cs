using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public PatientService(IGenericRepository<Patient> patientRepository,
            IMapper mapper, IConfiguration configuration, IGenericRepository<Contact> contactRepository,
            IGenericRepository<EmergencyContact> emrgencyContactRepository,
            IGenericRepository<Immunization> immunizationRepository,
            IGenericRepository<ImmunizationDocument> immunizationDocumentRepository,
            IGenericRepository<MedicalRecord> medicalRecordRepository,
            IGenericRepository<Medication> medicationRepository,
            IGenericRepository<PatientReferrer> patientReferrerRepository,
            IGenericRepository<Treatment> treatmentRepository, IGenericRepository<Visit> visitRepository,
            MedicalRecordDbContext dbContext, IGenericRepository<Employee> employeeRepository, IGenericRepository<Lab> labRepository)
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
				PatientId = patient.PatientId,
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

			return new ServiceResponse<IEnumerable<AssignedPatientsDTO>>(assignedPatientsDTOs, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
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

			return new ServiceResponse<ReadPatientDTO>(patientdata, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
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

			return new ServiceResponse<IEnumerable<ReadVisitHistoryDTO>>(visitsdata, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
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
