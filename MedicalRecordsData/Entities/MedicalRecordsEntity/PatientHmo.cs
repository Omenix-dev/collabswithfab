
using MedicalRecordsData.Entities.BaseEntity;
using System;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class PatientHmo : Base
    {
        public int HMOProviderId { get; set; }
        public int HMOPackageId { get; set; }
        public int PatientHMOId { get; set; }
        public DateTime MembershipValidity { get; set; }
        public string Notes { get; set; }
        public string PatientHMOCardDocumentUrl {get; set;}
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
