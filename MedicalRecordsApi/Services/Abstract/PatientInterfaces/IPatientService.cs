using MedicalRecordsApi.Models;
using MedicalRecordsApi.Utils;
using MedicalRecordsRepository.DTO.AuthDTO;
using System.Threading.Tasks;

using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalRecordsRepository.DTO.PatientDto;
using MedicalRecordsApi.Services;
using MedicalRecordsRepository.DTO.MedicalDto;

namespace MedicalRecordsApi.Services.Abstract.PatientInterfaces
{
	public interface IPatientService
	{
		/// <summary>
		/// This adds to the prescription data in the patients record
		/// </summary>
		/// <param name="prescriptionDTO"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
		Task<ServiceResponse<string>> AddPrescriptionAsync(CreatePatientPrescriptionDTO prescriptionDTO);
		/// <summary>
		/// This adds to the patient note in the patients record
		/// </summary>
		/// <param name="patientNoteDTO"></param>
		/// <returns>Returns a <see cref="ServiceResponse{string}"/> object.</returns>
		Task<ServiceResponse<string>> AddToPatientNoteAsync(CreatePatientNoteDTO patientNoteDTO);
		/// <summary>
		/// This gets the patients assigned to a particular doctor
		/// </summary>
		/// <returns>Returns a <see cref="ServiceResponse{List{AssignedPatientsDTO}}"/> object.</returns>
		Task<ServiceResponse<IEnumerable<AssignedPatientsDTO>>> GetAssignedPatientsAsync(int userId);
		/// <summary>
		/// This gets the patients record of a particular patient
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns>Returns a <see cref="ServiceResponse{ReadPatientDTO}"/> object.</returns>
		Task<ServiceResponse<ReadPatientDTO>> GetPatientDataAsync(int patientId);




        Task<ServiceResponse<string>> CreatePatientProfile(CreatePatientProfileDto profileDto, int userId);
		/// <summary>
		/// used to add the patient 
		/// </summary>
		/// <param name="patientDto"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<ServiceResponse<string>> AddPatient(CreatePatientRequestDto patientDto, int userId);
        Task<ServiceResponse<string>> UpdateContact(updateContactDto contactDto, int userId);
        Task<ServiceResponse<string>> UpdateEmergencyContact(UpdateEmergencyContactDto emergencyContactDto, int userId);

		//servoce for the medical records
		Task<ServiceResponse<List<MedicalRecordsDto>>> GetAllMedicalReportByPatientId(int patientId);
		Task<ServiceResponse<string>> DeleteMedicalReport(int RecordId);
		Task<ServiceResponse<string>> AddMedicalReport(MedicalRecordsDto MedicalRecords, int userId);

		//services for the immunization records
		Task<ServiceResponse<List<ImmunizationDto>>> GetAllImmunizatiobByPatientId(int patientId);
		Task<ServiceResponse<string>> DeleteImmunizationRecord(int RecordId);
		Task<ServiceResponse<string>> AddImmunizationRecords(ImmunizationDto ImmunizationRecords, int userId);
		// visation services
		Task<ServiceResponse<string>> DeleteVisitsRecord(int VisitId);
		Task<ServiceResponse<string>> AddPatientVistsRecords(PatientsVisitsDto PatientVisitsObj, int userId);
		Task<ServiceResponse<List<PatientsVisitsDto>>> GetAllVisitationByPatientId(int VisitaionId);
    }
}
