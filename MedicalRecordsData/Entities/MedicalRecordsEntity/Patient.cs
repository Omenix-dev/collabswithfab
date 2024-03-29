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
            PatientAssignmentHistory = new HashSet<PatientAssignmentHistory>();
        }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string StateOfOrigin { get; set; }
		public string Lga { get; set; }
		public string PlaceOfBirth { get; set; }
		public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
		public bool HasHmo { get; set; } = false;
		public bool IsReferred { get; set; }	

        //UserAuth
        public int UserId { get; set; }

		//Staff taking care of patient
		public int? NurseId { get; set; }
		public int? DoctorId { get; set; }

		//Clinic
		public int ClinicId { get; set; }


		//Navigation Properties
		public virtual Contact Contact { get; set; }
		public virtual EmergencyContact EmergencyContact { get; set; }

		public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
		public virtual ICollection<Immunization> Immunizations { get; set; }
		public virtual ICollection<Visit> Visits { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
        public virtual ICollection<PatientAssignmentHistory> PatientAssignmentHistory { get; set; }
		public virtual ICollection<PatientHmo> PatientHmo { get; set; }
    }
}
