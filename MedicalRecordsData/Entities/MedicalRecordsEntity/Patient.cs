using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Patient : Base
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string DateOfBirth { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Nationality { get; set; }
		public string StateOfOrigin { get; set; }
		public string LGA { get; set; }
		public string PlaceOfBirth { get; set; }
		public string MaritalStatus { get; set; }


		public Contact Contact { get; set; }
		public EmergencyContact EmergencyContact { get; set; }
		public List<MedicalRecord> MedicalRecord { get; set; }
		public List<Immunization> Immunization { get; set; }
		public List<Visit> Visit { get; set; }
		public PatientReferrer PatientReferrer { get; set; }
		public List<Treatment> Treatment { get; set; }
	}
}
