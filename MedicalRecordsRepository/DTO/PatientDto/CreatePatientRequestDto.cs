

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
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "NurseId is required")]
        public int NurseId { get; set; }
        [Required(ErrorMessage = "DoctorId is required")]
        public int DoctorId { get; set; }
    }

    public class UpdateMedicalStaffDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "NurseId is required")]
        public int NurseId { get; set; }
        [Required(ErrorMessage = "DoctorId is required")]
        public int DoctorId { get; set; }
    }
}
