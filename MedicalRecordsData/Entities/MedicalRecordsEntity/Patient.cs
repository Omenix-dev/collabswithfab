using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class Patient : Base
	{
		public Patient()
		{
			MedicalRecords = new HashSet<MedicalRecord>();
			Immunizations = new HashSet<Immunization>();
			Visits = new HashSet<Visit>();
			Treatments = new HashSet<Treatment>();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Nationality { get; set; }
		public string StateOfOrigin { get; set; }
		public string LGA { get; set; }
		public string PlaceOfBirth { get; set; }
		public string MaritalStatus { get; set; }

		//Staff taking care of patient
		public int NurseId { get; set; }
		public int DoctorId { get; set; }


		//Navigation Properties
		public virtual Contact Contact { get; set; }
		public virtual PatientReferrer PatientReferrer { get; set; }
		public virtual EmergencyContact EmergencyContact { get; set; }

		public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
		public virtual ICollection<Immunization> Immunizations { get; set; }
		public virtual ICollection<Visit> Visits { get; set; }
		public virtual ICollection<Treatment> Treatments { get; set; }
	}
}
