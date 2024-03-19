using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class PatientLabReport : Base
    {
        public string Subject { get; set; }
        public string LabFindings { get; set; }
        public List<PatientLabDocument> PatientLabDocuments { get; set; }
        public string PatientFullName { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
        public string LabRequest { get; set; }
        public int LabRequestId { get; set; }
        public DateTime LabRequestDate { get; set; }
        public int PatientUserId { get; set; }
        public int PatientId { get; set; }
        public int LabTechnicianEmployeeId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; }
    }
}
