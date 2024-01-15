using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsData.Entities
{
    public class UserRoles
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
