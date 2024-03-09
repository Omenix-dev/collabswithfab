using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsApi.Utils
{
    public class JwtTokenReturn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string LoginTime { get; set; }
        public int ClinicId { get; set; }
        //public string ClinicName { get; set; }
        //public string ClinicAddress { get; set; }

        //public string HomeLink { get; set; }
    }
}
