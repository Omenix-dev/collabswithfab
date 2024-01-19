using System;
using System.Collections.Generic;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public partial class Role
    {
        public Role()
        {
            Employees = new HashSet<Employee>();
            Userroles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int ModifiedBy { get; set; }
        public string ActionTaken { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<UserRole> Userroles { get; set; }
    }
}
