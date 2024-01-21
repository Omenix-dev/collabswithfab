using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

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
	}
}
