using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public class User: Base
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }

        public Boolean IsSettings { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public string HomeLink { get; set; }
        [NotMapped]
        public string ClinicName { get; set; }
        [NotMapped]
        public string ClinicAddress { get; set; }
    }
}
