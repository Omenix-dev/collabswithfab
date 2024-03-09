using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public partial class Role
    {
        //public Role()
        //{
        //    Employees = new HashSet<Employee>();
        //}

        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; } = 1;
        public string Description { get; set; }
        public List<RolePrivilegeAccess> RolePrivilegeAccesses { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }
    }

    public class RolePrivilegeAccess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; } = 1;
        [ForeignKey(nameof(RoleId))]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; }
    }
}
