

using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class CreatePatientRequestDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        public string StateOfOrigin { get; set; }
        public string Lga { get; set; }
        public string PlaceOfBirth { get; set; }

        [Required(ErrorMessage = "Marital status is required")]
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; }
        public int ClinicId { get; set; }
        public string PictureUrl { get; set; }
    }

    public class UpdateMedicalStaffDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        public int? PatientId { get; set; }
        public int? NurseEmployeeId { get; set; }
        public int? DoctorEmployeeId { get; set; }
        public int ClinicId { get; set; }   
    }
    public class UpdatePatientDto 
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage ="The Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [DateNotGreaterThanNow(ErrorMessage = "Date should not be greater than current date")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage ="the email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Phone(ErrorMessage ="the is not a valid phone number") ]
        public string PhoneNumber { get; set; }
        public string StateOfOrigin { get; set; }
        public string Lga { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
    }
}
