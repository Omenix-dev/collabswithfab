using MedicalRecordsData.Entities.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string ActionTaken { get; set; }
        public string Name{get; set;}
        public string Location{get; set;}
        public string DateEstablished{get; set;}
        public string Mandate{get; set;}

    }
}
