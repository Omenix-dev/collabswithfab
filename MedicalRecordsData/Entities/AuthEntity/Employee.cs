using MedicalRecordsData.Entities.MedicalRecordsEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public partial class Employee
    {
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
        public List<EmployeePrivilegeAccess> EmployeePrivilegeAccesses { get; set; }
        public string WorkGrade { get; set; }
        public string ResumptionDate { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string Signature { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public string AccountStatus { get; set; }
        public DateTime? OnboardingDate { get; set; }

        public Employee(AddInformationDTO profile)
        {
            RoleId = Convert.ToInt32(profile.RoleId);
            Email = profile.Email;
            Department = profile.Department;
            WorkGrade = profile.WorkGrade;
            Title = profile.Title;
            FirstName = profile.FirstName;
            MiddleName = profile.MiddleName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            DateOfBirth = profile.DateOfBirth;
            PlaceOfBirth = profile.PlaceOfBirth;
            MaritalStatus = profile.MaritalStatus;
            MotherMaidenName = profile.MotherMaidenName;
            WeddingAnniversary = profile.WeddingAnniversary;
            StateId = Convert.ToInt32(profile.StateId);
            NationalityId = Convert.ToInt32(profile.NationalityId);
            ReligionId = Convert.ToInt32(profile.ReligionId);
            Signature = profile.Signature;
            Bvn = profile.Bvn;
            Nin = profile.Nin;
            SalaryAccountNumber = profile.SalaryAccountNumber;
            SalaryDomiciledBank = profile.SalaryDomiciledBank;
            ResumptionDate = profile.ResumptionDate;
            ActionTaken = "Record Updated";
            Status = 1;
            ModifiedAt = DateTime.Now;
            ModifiedBy = UserId;
            OnboardingDate = DateTime.Now;
        }

        public Employee(ProfileUpdateDTO profile)
        {
            Email = profile.Email;
            Title = profile.Title;
            FirstName = profile.FirstName;
            MiddleName = profile.MiddleName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            DateOfBirth = profile.DateOfBirth;
            PlaceOfBirth = profile.PlaceOfBirth;
            MaritalStatus = profile.MaritalStatus;
            MotherMaidenName = profile.MotherMaidenName;
            WeddingAnniversary = profile.WeddingAnniversary;
            StaffId = profile.StaffId;
            StateId = profile.StateId;
            NationalityId = profile.NationalityId;
            ReligionId = profile.ReligionId;
            StaffId = profile.StaffId;
            Bvn = profile.Bvn;
            Nin = profile.Nin;
            SalaryAccountNumber = profile.SalaryAccountNumber;
            SalaryDomiciledBank = profile.SalaryDomiciledBank;
            ClinicId = profile.ClinicId;
            //ProfilePicture = profile.ProfilePicture;
            RoleId = profile.RoleId;
            EmployeePrivilegeAccesses = profile.EmployeePrivilegeAccesses.Select(dto => new EmployeePrivilegeAccess(dto)).ToList();
            WorkGrade = profile.WorkGrade;
            ResumptionDate = profile.ResumptionDate;
            Signature = profile.Signature;
            Designation = profile.Designation;
            Username = profile.Username;
            UserId = profile.UserId;
            IsSuperAdmin = profile.IsSuperAdmin;
            AccountStatus = profile.AccountStatus;
            ActionTaken = "Record Updated";
            Status = 1;
            ModifiedAt = DateTime.Now;
        }

        public Employee(BasicProfileCreateDTO basicProfile)
        {
            StaffId = basicProfile.StaffId;
            Designation = basicProfile.Designation;
            UserId = basicProfile.UserId;
            RoleId = basicProfile.RoleId;
            EmployeePrivilegeAccesses = basicProfile.EmployeePrivilegeAccesses.Select(dto => new EmployeePrivilegeAccess(dto)).ToList();
            IsSuperAdmin = basicProfile.IsSuperAdmin;
            CreatedAt = DateTime.Now;
            Status = 1;
            CreatedBy = UserId;
            ActionTaken = "Profile Creation";
        }

        // public Employee(UpdateStatusDTO statusDTO)
        // {
        //     Username = statusDTO.Username;
        //     AccountStatus = statusDTO.AccountStatus;
        //     ModifiedAt = DateTime.Now;
        //     Status = 1;
        //     ModifiedBy = UserId ?? 0;
        //     ActionTaken = "Profile Updated";
        // }

        public Employee()
        {
        }
    }

    public class EmployeePrivilegeAccess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public EmployeePrivilegeAccess(EmployeePrivilegeAccessRequestDTO privilege)
        {
            Name = privilege.Name;
            Status = 1;
            CreatedAt = DateTime.Now;
        }

        // public EmployeePrivilegeAccess(EmployeePrivilegeAccess privilege)
        // {
        //     Id = privilege.Id;
        //     Name = privilege.Name;
        //     Status = privilege.Status;
        //     EmployeeId = privilege.EmployeeId;
        //     CreatedBy = privilege.CreatedBy;
        //     ModifiedBy = privilege.ModifiedBy;
        //     CreatedAt = privilege.CreatedAt;
        //     ModifiedAt = privilege.ModifiedAt;
        // }

        public EmployeePrivilegeAccess()
        {

        }
    }

    public class EmployeePrivilegeAccessRequestDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Name should be between 5 and 200 characters.")]
        public string Name { get; set; }
    }

    public class BasicProfileCreateDTO
    {
        [Required(ErrorMessage = "Staff Id is required.")]
        public string StaffId { get; set; }
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "RolePrivilegeAccesses is required.")]
        public List<EmployeePrivilegeAccessRequestDTO> EmployeePrivilegeAccesses { get; set; }
        [Required(ErrorMessage = "IsSuperAdmin is required.")]
        public bool? IsSuperAdmin { get; set; }
    }

    public class ProfileUpdateDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title should be between 3 and 100 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First Name should be between 3 and 100 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Middle Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Middle Name should be between 3 and 100 characters.")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last Name should be between 3 and 100 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "SalaryAccountNumber is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "SalaryAccountNumber should be 10 characters.")]
        public string SalaryAccountNumber { get; set; }
        [Required(ErrorMessage = "Salary Domiciled Bank is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Salary Domiciled Bank should be between 3 and 100 characters.")]
        public string SalaryDomiciledBank { get; set; }
        [Required(ErrorMessage = "Nin is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Nin should be 11 characters.")]
        public string Nin { get; set; }
        [Required(ErrorMessage = "Bvn is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Bvn should be 11 characters.")]
        public string Bvn { get; set; }
        [Required(ErrorMessage = "StaffId is required.")]
        public string StaffId { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Date Of Birth must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Place Of Birth is required.")]
        public string PlaceOfBirth { get; set; }
        [Required(ErrorMessage = "Marital Status is required.")]
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "State Id is required.")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Nationality Id is required.")]
        public int NationalityId { get; set; }
        [Required(ErrorMessage = "Religion Id is required.")]
        public int ReligionId { get; set; }
        [Required(ErrorMessage = "Clinic Id is required.")]
        public int ClinicId { get; set; }
        [Required(ErrorMessage = "MotherMaiden Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "MotherMaiden Name should be between 3 and 100 characters.")]
        public string MotherMaidenName { get; set; }
        [Required(ErrorMessage = "Wedding Anniversary is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Wedding Anniversary must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string WeddingAnniversary { get; set; }
        // [Required(ErrorMessage = "Profile Picture is required.")]
        // public string ProfilePicture { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "RolePrivilegeAccesses is required.")]
        public List<EmployeePrivilegeAccessRequestDTO> EmployeePrivilegeAccesses { get; set; }
        [Required(ErrorMessage = "Work Grade is required.")]
        public string WorkGrade { get; set; }
        [Required(ErrorMessage = "Resumption Date is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Resumption Date must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string ResumptionDate { get; set; }
        [Required(ErrorMessage = "Signature is required.")]
        public string Signature { get; set; }
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username should be between 6 and 100 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "IsSuperAdmin is required.")]
        public bool? IsSuperAdmin { get; set; }
        [Required(ErrorMessage = "AccountStatus is required.")]
        public string AccountStatus { get; set; }
    }

    public class AddInformationDTO
    {
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Department should be between 3 and 100 characters.")]
        public string Department { get; set; }
        [Required(ErrorMessage = "WorkGrade is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "WorkGrade should be between 3 and 100 characters.")]
        public string WorkGrade { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title should be between 3 and 100 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First Name should be between 3 and 100 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Middle Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Middle Name should be between 3 and 100 characters.")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last Name should be between 3 and 100 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Date Of Birth must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Place Of Birth is required.")]
        public string PlaceOfBirth { get; set; }
        [Required(ErrorMessage = "Marital Status is required.")]
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "MotherMaiden Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "MotherMaiden Name should be between 3 and 100 characters.")]
        public string MotherMaidenName { get; set; }
        [Required(ErrorMessage = "Wedding Anniversary is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Wedding Anniversary must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string WeddingAnniversary { get; set; }
        [Required(ErrorMessage = "State Id is required.")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Nationality Id is required.")]
        public int NationalityId { get; set; }
        [Required(ErrorMessage = "Religion Id is required.")]
        public int ReligionId { get; set; }
        [Required(ErrorMessage = "Signature is required.")]
        public string Signature { get; set; }
        [Required(ErrorMessage = "Bvn is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Bvn should be 11 characters.")]
        public string Bvn { get; set; }
        [Required(ErrorMessage = "Nin is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Nin should be 11 characters.")]
        public string Nin { get; set; }
        [Required(ErrorMessage = "Salary Account Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "SalaryAccountNumber should be 10 characters.")]
        public string SalaryAccountNumber { get; set; }
        [Required(ErrorMessage = "Salary Domiciled Bank is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Salary Domiciled Bank should be between 3 and 100 characters.")]
        public string SalaryDomiciledBank { get; set; }
        [Required(ErrorMessage = "Resumption Date is required.")]
        [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Resumption Date must be in the format DD-MM-YYYY and year should not be more than 2030.")]
        public string ResumptionDate { get; set; }
    }
}