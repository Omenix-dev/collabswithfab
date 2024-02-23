using MedicalRecordsData.Entities.BaseEntity;
using MedicalRecordsData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public partial class PatientAssignmentHistory : Base
    {
        public int PatientId { get; set; }
        public int NurseId { get; set; }
        public int DoctorId { get; set; }
        public PatientCareType CareType { get; set; }
    }
}
