namespace MedicalRecordsApi.Models.DTO.Request
{
	public class CreatePatientNoteDto
	{
		public int NurseId { get; set; }
		public int DoctorId { get; set; }
		public int TreatmentId { get; set; }
		public string AdditonalNoteOnTreatment { get; set; }
	}
}
