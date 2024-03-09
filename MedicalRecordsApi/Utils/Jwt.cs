using System;

namespace MedicalRecordsApi.Utils
{
    public class Jwt
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Jwt(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }
    }
}
