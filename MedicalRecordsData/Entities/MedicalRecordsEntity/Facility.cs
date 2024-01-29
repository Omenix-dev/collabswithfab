using MedicalRecordsData.Entities.BaseEntity;
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

    public enum FacilityType
    {
        Bed,
        Equipment,
        Ambulance
    }
}
