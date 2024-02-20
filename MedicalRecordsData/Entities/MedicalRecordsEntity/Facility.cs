using MedicalRecordsData.Entities.BaseEntity;
using MedicalRecordsData.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public partial class Facility : Base
    {
        public FacilityType FacilityType { get; set; }
        public string Name { get; set; }
        public bool IsOccupied { get; set; }
    }
}
