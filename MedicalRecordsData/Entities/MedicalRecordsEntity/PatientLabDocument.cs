using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class PatientLabDocument : Base
    {
        public string DocName { get; set; }
        public string DocPath { get; set; }
        [ForeignKey(nameof(PatientLabReportId))]
        public int PatientLabReportId { get; set; }
        public PatientLabReport PatientLabReport { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; }
    }
}
