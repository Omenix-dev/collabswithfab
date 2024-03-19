using System.ComponentModel.DataAnnotations;
using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class GetAllPatientsDto
    {
        public int PatientId { get; set; }
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
        public int NurseId { get; set; }
        public int DoctorId { get; set; }
    }
}
