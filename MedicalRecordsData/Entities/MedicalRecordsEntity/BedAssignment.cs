using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public partial class BedAssignment : Base
    {
        public int PatientId { get; set; }
        public int FacilityId { get; set; }
    }
}
