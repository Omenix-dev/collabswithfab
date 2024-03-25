using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class Bed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public string ActionTaken { get; set; }
    }
}