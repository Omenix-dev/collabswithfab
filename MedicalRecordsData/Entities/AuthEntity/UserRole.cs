using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public class UserRole
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
