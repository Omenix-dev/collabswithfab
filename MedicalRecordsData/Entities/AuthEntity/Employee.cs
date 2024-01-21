using System;
using System.Collections.Generic;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public partial class Employee
    {
        public Employee()
        {
            Userroles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string ActionTaken { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string SalaryAccountNumber { get; set; }
        public string SalaryDomiciledBank { get; set; }
        public string Nin { get; set; }
        public string Bvn { get; set; }
        public string StaffId { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public int StateId { get; set; }
        public string AuthenticationToken { get; set; }
        public int NationalityId { get; set; }
        public int ReligionId { get; set; }
        public int? ClinicId { get; set; }
        public string MotherMaidenName { get; set; }
        public string WeddingAnniversary { get; set; }
        public string ProfilePicture { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int? RoleId { get; set; }


		public string WorkGrade { get; set; }
		public string ResumptionDate { get; set; }
		public DateTime? LastLoginTime { get; set; }
		public string Signature { get; set; }
		public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

		public virtual Role Role { get; set; }
        public virtual ICollection<UserRole> Userroles { get; set; }
    }
}
