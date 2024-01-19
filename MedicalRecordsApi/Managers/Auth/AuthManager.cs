using AutoMapper;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Managers.Auth
{
    public class AuthManager : IAuthManager
    {
        #region fields
        private readonly IGenericRepository<User> repository;
        private readonly ILogger<AuthManager> log;
        private readonly IMapper map;
        private readonly IConfiguration configuration;
        private readonly HttpContext _httpContextAccessor;


        #endregion
        public AuthManager(IGenericRepository<User> repository, ILogger<AuthManager> log, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            map = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor.HttpContext;
        }


        public async Task<TokenDTO> LogUserIn(UserDTO model)
        {
            if (model == null) return null;
            User user = null;
            var userContext = _httpContextAccessor.Connection;
            //var userIdentity = (ClaimsIdentity)userContext.Identity;
            //var claim = userIdentity.Claims.ToList();
            //var roleClaimType = userIdentity.RoleClaimType;
            //var roles = claim.Where(c => c.Type == ClaimTypes.Role).Select(d => d.Value).ToList();
            log.LogInformation($"=>> {userContext}");
            try
            {

                user = await repository.FirstOrDefault(u => u.Email == model.Email);
                if (user == null) return null;
            }catch(DbUpdateException e)
            {
                log.LogError($"{e.Message}");
            }


            var verifyPwd = AuthUtil.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);
            if (!verifyPwd) return null;

            var claims = new ClaimsIdentity(new[] { new Claim("id", $"{user.Id}"), new Claim(ClaimTypes.Email, model.Email), new Claim(ClaimTypes.Role, user.Role), new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName) });
            var jwtSecret = configuration["JwtSettings:Secret"];
            var token = AuthUtil.GenerateJwtToken(jwtSecret, claims);
            claims.AddClaim(new Claim("token", token));

            var refreshToken = AuthUtil.GenerateRefreshToken();

            // Save tokens to DB
            user.Token = token;
            await repository.Update(user);
            return new TokenDTO { Token = token, RefreshToken=refreshToken};
            //throw new NotImplementedException("h");
        }


        public JwtTokenReturn SignInUser(User user)
        {
            var claims = new ClaimsIdentity(new[] { new Claim("id", $"{user.Id}"), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, user.Role), new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName) });
            var jwtSecret = configuration["JwtSettings:Secret"];
            var token = AuthUtil.GenerateJwtToken(jwtSecret, claims, 120);
            claims.AddClaim(new Claim("token", token));

            //var refreshToken = AuthUtil.GenerateRefreshToken();

            // Save tokens to DB
            //user.Token = token;

          
            JwtTokenReturn _jwt1 = new JwtTokenReturn
            {
                FullName = user.FirstName + " " + user.LastName,
                Role = user.Role,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                LoginTime = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                Token = token,
                ClinicName=user.ClinicName,
                ClinicAddress=user.ClinicAddress,
                HomeLink = user.HomeLink
            };

            return _jwt1;
            
            //await repository.Update(user);
            //return new TokenDTO { Token = token, RefreshToken = refreshToken };
            //throw new NotImplementedException("h");
        }

        public async Task<(User user, string message)> RegisterUser(UserDTO model)
        {
           
            var userExists = await repository.FirstOrDefault(r => r.Email == model.Email);
            if (userExists != null) return (user: null, message: $"User {userExists.Email} exists already!");
             AuthUtil.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userDetails = new User
            {
                CreatedAt = DateTime.UtcNow,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = model.Email,
            };

            var newuser = await repository.Insert(userDetails);

            return (user:newuser, message:"User created successfully.");
        }
    }
    public interface IAuthManager
    {
        Task<TokenDTO> LogUserIn(UserDTO model);
        JwtTokenReturn SignInUser(User user);
        Task<(User user, string message)> RegisterUser(UserDTO model);
    }
}
