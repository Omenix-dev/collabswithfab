using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class PatientHMODto
    {
        public int HMOProviderId { get; set; }
        public int HMOPackageId { get; set; }
        public int PatientHMOId { get; set; }
        public DateTime MembershipValidity { get; set; }
        public string Notes { get; set; }
        public string PatientHMOCardDocumentUrl { get; set; }
        public int PatientId { get; set; }
    }
    public class UpdatePatientHMODto
    {
        public int HMOProviderId { get; set; }
        public int HMOPackageId { get; set; }
        public int PatientHMOId { get; set; }
        public DateTime MembershipValidity { get; set; }
        public string Notes { get; set; }
        public string PatientHMOCardDocumentUrl { get; set; }
        public int Id { get; set; }
    }
}
