using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
    public class AssignPatientBed
    {
        public int Id { get; set; }
        public int BedId { get; set; }
        public string PatientAssignedName { get; set; }
        public int AssignerUserId { get; set; }
        public int PatientAssignedId { get; set; }
        public int PatientAssignedUserId { get; set; }
        public string AssignNote { get; set; }
        public int UnAssignerUserId { get; set; }
        public DateTime BedAssignDate { get; set; } = DateTime.Now;
        public DateTime BedUnAssignDate { get; set; }
        public int Status { get; set; }
        public string ActionTaken { get; set; }
        public int AssignedBy { get; set; }
        public int UnAssignedBy { get; set; }
    }
}