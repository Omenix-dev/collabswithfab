using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsRepository.DTO.AuthDTO
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
