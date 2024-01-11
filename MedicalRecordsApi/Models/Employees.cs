using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_structure.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string AuthenticationToken { get; set; }
        public string resumptiondate { get; set; }
        public int MdaId { get; set; }
        public int RoleId { get; set; }
    }
}
