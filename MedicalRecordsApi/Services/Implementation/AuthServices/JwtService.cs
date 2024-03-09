using MedicalRecordsApi.Services.Abstract.AuthServices;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace MedicalRecordsApi.Services.Implementation.AuthServices
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public Jwt GetJwt(User user)
        {
            //_cacheProvider.SaveCurrentUserToCache(user);
            return GenerateJwt(user);
        }

        #region Jwt Private Methods
        Jwt GenerateJwt(User user)
        {
            // var token = new Token();
            // _configuration.GetSection(nameof(Token)).Bind(token);
            var claims = Claims(user);
            var securityToken = SecurityToken(claims);

            // Parse the token expiry string to a double
            if (!double.TryParse(_configuration["Jwt:Expiry"], out double tokenExpiryHours))
            {
                throw new InvalidOperationException("Invalid token expiry format");
            }
            var jwt = new Jwt(new JwtSecurityTokenHandler().WriteToken(securityToken),
            DateTime.UtcNow.AddDays(1));
            return jwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        Claim[] Claims(User user)
        {
            return new[]
            {
                new Claim("id", $"{user.Id}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,user.RoleId.ToString())
            };
        }
        JwtSecurityToken SecurityToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Parse the token expiry string to a double
            if (!double.TryParse(_configuration["Jwt:Expiry"], out double tokenExpiryHours))
            {
                throw new InvalidOperationException("Invalid token expiry format");
            }

            var expirationDate = DateTime.UtcNow.AddHours(tokenExpiryHours);


            return new JwtSecurityToken
            (
                audience: _configuration["Jwt:Audience"],
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: expirationDate,
                signingCredentials: signingCredentials
            );
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string key)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public static int? ValidateJwtToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return account id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        #endregion

    }
}