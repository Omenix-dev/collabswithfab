using System;
using System.Collections.Generic;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public partial class Department
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string ActionTaken { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string DateEstablished { get; set; }
        public string Mandate { get; set; }
    }
}
