using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadContactDetailsDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime DateOfVisit { get; set; }
        public string StateOfResidence { get; set; }
        public string LgaResidence { get; set; }
        public string City { get; set; }
        public string HomeAddress { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Email { get; set; }

        public int PatientId { get; set; }
    }
}
