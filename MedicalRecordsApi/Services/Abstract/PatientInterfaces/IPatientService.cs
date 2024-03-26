using MedicalRecordsRepository.DTO.AuthDTO;
using System.Threading.Tasks;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using MedicalRecordsRepository.DTO.PatientDto;
using MedicalRecordsApi.Services;
using MedicalRecordsRepository.DTO.MedicalDto;
using MedicalRecordsRepository.DTO;
using MedicalRecordsData.Migrations;
using System;

namespace MedicalRecordsApi.Services.Abstract.PatientInterfaces
{
	public interface IPatientService
	{
        /// <summary>
        /// This adds to the prescription data in the patients record
        /// </summary>
        /// <param name="patientId"></param>
		/// <param name="visitId"></param>
        /// <param name="prescriptionDto"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        Task<ServiceResponse<string>> AddPrescriptionAsync(int patientId, int visitId, CreatePatientPrescriptionDto prescriptionDto);
		/// <summary>
		/// This adds to the patient note in the patients record
		/// </summary>
		/// <param name="patientId"></param>
		/// <param name="patientNoteDto"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
		Task<ServiceResponse<string>> AddToPatientNoteAsync(int patientId, CreatePatientNoteDto patientNoteDto);
		/// <summary>
		/// This gets the patients admission history for quick evaluation
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns>Returns a <see cref="ServiceResponse{IEnumerable{ReadVisitHistoryDTO}}"/> object.</returns>
		Task<ServiceResponse<IEnumerable<ReadVisitHistoryDto>>> GetAllAdmissionHistoryAsync(int patientId);

        /// <summary>
        /// This gets the patients assigned to a particular doctor
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{PaginatedList{AssignedPatientsDTO}}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<PaginatedList<AssignedPatientsDto>>> GetAssignedPatientsAsync(int pageIndex, int pageSize, int userId);
        /// <summary>
        /// This gets the lab note of a patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="labrequestId"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        Task<ServiceResponse<ReadPatientLabReport>> GetLabReportAsync(int patientId, int labrequestId);
        /// <summary>
        /// This gets nurses notes for a particular visit of a patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <returns>Returns a <see cref="ServiceResponse{ReadNurseNotesDTO}"/> object.</returns>
        Task<ServiceResponse<ReadNurseNotesDto>> GetNurseNoteAsync(int patientId, int visitId);
		/// <summary>
		/// This gets the patients record of a particular patient
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadPatientDTO}"/> object.</returns>
		Task<ServiceResponse<ReadPatientDto>> GetPatientDataAsync(int patientId);
        /// <summary>
        /// This adds to the lab table thereby referring the patient to the lab
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="visitId"></param>
        /// <param name="labReferDto"></param>
        /// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
        Task<ServiceResponse<string>> ReferPatientAsync(int patientId, int visitId, CreateLabReferDto labReferDto);
        /// <summary>
        /// This gets the patients contact details
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{ReadContactDetailsDto}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<ReadContactDetailsDto>> GetContactDetailsAsync(int patientId);
        /// <summary>
        /// This gets the patients emergency contact details
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{ReadEmergencyContactDetailsDto}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<ReadEmergencyContactDetailsDto>> GetEmergencyContactDetailsAsync(int patientId);
        /// <summary>
        /// This gets the patients medical record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadMedicalRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        ServiceResponse<IEnumerable<ReadMedicalRecordDto>> GetMedicalRecordAsync(int patientId);
        /// <summary>
        /// This gets the patients immunization record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadImmunizationRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<IEnumerable<ReadImmunizationRecordDto>>> GetImmunizationRecordAsync(int patientId);
        /// <summary>
        /// This gets the patients visit record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadVisitHistoryDto}}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<IEnumerable<ReadVisitHistoryDto>>> GetVisitRecordAsync(int patientId);
        /// <summary>
        /// This gets the patients treatment record
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{IEnumerable{ReadTreatmentRecordDto}}</cref>
        ///     </see>
        ///     object.</returns>
        Task<ServiceResponse<IEnumerable<ReadTreatmentRecordDto>>> GetTreatmentRecordAsync(int patientId);
        /// <summary>
        /// This gets the filtered result of parameters sent
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="startDate"></param>
        /// <param name="gender"></param>
        /// <param name="email"></param>
        /// <param name="endDate"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Returns a <see>
        ///         <cref>ServiceResponse{PaginatedList{ReadPatientDto}}</cref>
        ///     </see>
        ///     object.</returns>
        ServiceResponse<PaginatedList<ReadPatientDto>> GetFilteredPatientInfo(string firstName, string lastName, string gender, string email, DateTime? startDate, DateTime? endDate, string phoneNumber, int pageIndex, int pageSize);
        Task<ServiceResponse<string>> CreatePatientProfile(CreatePatientProfileDto profileDto, int userId);
		/// <summary>
		/// used to add the patient 
		/// </summary>
		/// <param name="patientDto"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<ServiceResponse<object>> AddPatient(CreatePatientRequestDto patientDto, int userId);
        Task<ServiceResponse<object>> UpdateContact(UpdateContactDto contactDto, int userId);
        Task<ServiceResponse<object>> UpdateEmergencyContact(UpdateEmergencyContactDto emergencyContactDto, int userId);

		//servoce for the medical records
		Task<ServiceResponse<List<ResponseMedicalRecordsDto>>> GetAllMedicalReportByPatientId(int patientId);
		Task<ServiceResponse<string>> DeleteMedicalReport(int recordId);
		Task<ServiceResponse<object>> AddMedicalReport(MedicalRecordsDto medicalRecords, int userId);

		//services for the immunization records
		Task<ServiceResponse<List<ResponseImmunizationDto>>> GetAllImmunizatiobByPatientId(int patientId);
		Task<ServiceResponse<string>> DeleteImmunizationRecord(int recordId);
		Task<ServiceResponse<object>> AddImmunizationRecords(ImmunizationDto immunizationRecords, int userId);
		// visation services
		Task<ServiceResponse<object>> DeleteVisitsRecord(int visitId);
		Task<ServiceResponse<object>> AddPatientVistsRecords(PatientsVisitsDto patientVisitsObj, int userId);
		Task<ServiceResponse<List<ResponsePatientsVisitsDto>>> GetAllVisitationByPatientId(int PatientId);
        Task<ServiceResponse<object>> UpdateMedicalStaffByPatientId(UpdateMedicalStaffDto updateMedicalStaffDto, int userId);
        ServiceResponse<PaginatedList<GetAllPatientsDto>> GetAllPatient(int pageIndex, int pageSize, int ClinicId);
        ServiceResponse<GetAllPatientsDto> GetAllPatientById(int patientId);
        ServiceResponse<PaginatedList<GetAllNurseDto>> GetAllNurses(int pageIndex, int pageSize, int clinicId);
        Task<ServiceResponse<object>> UpdatePatient(UpdatePatientDto updatePatientDto, int userId);
        ServiceResponse<object> EndOfVisit(int patientId, int userId);
    }
}
