using MedicalRecordsData.Entities.MedicalRecordsEntity;
using System.Collections.Generic;
using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadPatientDTO
	{

		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string StateOfOrigin { get; set; }
		public string LGA { get; set; }
		public string PlaceOfBirth { get; set; }
		public string MaritalStatus { get; set; }

		public string PatientId { get; set; }

		//Staff taking care of patient
		public string NurseName { get; set; }
		public string DoctorName { get; set; }

		//Navigation Properties
		public Contact Contact { get; set; }
		public PatientReferrer PatientReferrer { get; set; }
		public EmergencyContact EmergencyContact { get; set; }

		public List<MedicalRecord> MedicalRecords { get; set; }
		public List<Immunization> Immunizations { get; set; }
		public List<Visit> Visits { get; set; }
		public List<Treatment> Treatments { get; set; }
	}
}
