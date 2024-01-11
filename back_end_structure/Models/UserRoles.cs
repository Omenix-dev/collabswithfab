using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_structure.Models
{
    public class UserRoles
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
