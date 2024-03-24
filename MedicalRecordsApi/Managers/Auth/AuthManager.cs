using AutoMapper;
using log4net;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Services.Abstract.AuthServices;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using MedicalRecordsRepository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Managers.Auth
{
    public class AuthManager : IAuthManager
    {
        #region fields
        private readonly IGenericRepository<User> _repository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IJwtService _tokenService;
        private readonly ILogger<AuthManager> _log;
        private readonly IMapper _map;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContextAccessor;


        #endregion
        public AuthManager(IGenericRepository<User> repository, ILogger<AuthManager> log, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IJwtService tokenService, IGenericRepository<Employee> employeeRepository, IGenericRepository<Role> roleRepository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._log = log ?? throw new ArgumentNullException(nameof(log));
            _map = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._configuration = configuration;
            _httpContextAccessor = httpContextAccessor.HttpContext;
            _tokenService = tokenService;
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        public async Task<ServiceResponse<AuthResponseDTO>> Login(UserDto credential)
        {
            try
            {
                if (credential == null)
                {
                    return new ServiceResponse<AuthResponseDTO>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
                }

                _log.LogInformation("Generating token for the user{0} ...", credential);
                var user = await GetUserByCredential(credential);
                if (user == null || !VerifyPasswordHash(credential.Password, user.Data.PasswordHash, user.Data.PasswordSalt))
                    return new ServiceResponse<AuthResponseDTO>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);

                Jwt jwt = _tokenService.GetJwt(user.Data);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.Data.Token = jwt.Token;
                user.Data.RefreshToken = refreshToken;

                var profile = await _employeeRepository.Query()
                    .Include(e => e.EmployeePrivilegeAccesses)
                    .FirstOrDefaultAsync(m => m.Email == user.Data.Email);

                var role = await _roleRepository.Query()
                    .FirstOrDefaultAsync(m => m.Id == user.Data.RoleId);

                if (profile != null)
                {
                    profile.AuthenticationToken = user.Data.Token;
                    profile.LastLoginTime = DateTime.Now;

                    await _employeeRepository.Update(profile);
                }

                await _repository.Update(user.Data);

                AuthResponseDTO authResponseDTO = new AuthResponseDTO();
                authResponseDTO.jwt = jwt;
                authResponseDTO.RefreshToken = refreshToken;
                authResponseDTO.FirstName = profile.FirstName;
                authResponseDTO.LastName = profile.LastName;
                authResponseDTO.Role = role.Name;
                authResponseDTO.Username = profile.Username;
                authResponseDTO.ClinicId = profile.ClinicId;

                return new ServiceResponse<AuthResponseDTO>(authResponseDTO, InternalCode.Success);
            }
            catch (Exception)
            {
                return new ServiceResponse<AuthResponseDTO>(null, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }

        private async Task<ServiceResponse<User>> GetUserByCredential(UserDto credential)
        {
            if (credential == null)
            {
                return new ServiceResponse<User>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var user = await _repository.Query()
                                        .Where(x => x.Username == credential.UsernameOrEmail || x.Email == credential.UsernameOrEmail).FirstOrDefaultAsync();

            if (user == null)
            {
                return new ServiceResponse<User>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<User>(user, InternalCode.Success);
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (storedHash == null) throw new ArgumentNullException(nameof(storedHash));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid hash length", nameof(storedHash)); // Assuming storedHash length is 64 when converted to Base64
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid salt length", nameof(storedSalt));

            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            // Convert computed hash to Base64 string
            var computedHashString = Convert.ToBase64String(computedHash);

            // Convert storedHash to Base64 string
            var storedHashString = Convert.ToBase64String(storedHash);

            // Compare the two Base64 strings
            return computedHashString == storedHashString;
        }

        public async Task<TokenDto> LogUserIn(UserDto model)
        {
            if (model == null) return null;
            Employee user = null;
            Role role = null;
            var userContext = _httpContextAccessor.Connection;
            //var userIdentity = (ClaimsIdentity)userContext.Identity;
            //var claim = userIdentity.Claims.ToList();
            //var roleClaimType = userIdentity.RoleClaimType;
            //var roles = claim.Where(c => c.Type == ClaimTypes.Role).Select(d => d.Value).ToList();
            _log.LogInformation($"=>> {userContext}");
            try
            {

                user = await _employeeRepository.Query().FirstOrDefaultAsync(u => u.Email == model.UsernameOrEmail || u.Username == model.UsernameOrEmail);
                if (user == null) return null;

                role = await _roleRepository.Query().FirstOrDefaultAsync(x => x.Id == user.RoleId);
            }
            catch (DbUpdateException e)
            {
                _log.LogError($"{e.Message}");
            }


            var verifyPwd = AuthUtil.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);
            if (!verifyPwd) return null;

            var claims = new ClaimsIdentity(new[] { new Claim("id", $"{user.Id}"), new Claim(ClaimTypes.Email, model.UsernameOrEmail), new Claim(ClaimTypes.Role, role.Name), new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName) });
            var jwtSecret = _configuration["JwtSettings:Secret"];
            var token = AuthUtil.GenerateJwtToken(jwtSecret, claims);
            claims.AddClaim(new Claim("token", token));

            var refreshToken = AuthUtil.GenerateRefreshToken();

            // Save tokens to DB
            user.AuthenticationToken = token;
            await _employeeRepository.Update(user);
            return new TokenDto { Token = token, RefreshToken = refreshToken };
            //throw new NotImplementedException("h");
        }


        public async Task<JwtTokenReturn> SignInUserAsync(Employee user)
        {
            var role = await _roleRepository.Query().FirstOrDefaultAsync(x => x.Id == user.RoleId);

            var claims = new ClaimsIdentity(new[] { new Claim("id", $"{user.Id}"),
                new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, role.Name),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName) });

            var jwtSecret = _configuration["JwtSettings:Secret"];
            var token = AuthUtil.GenerateJwtToken(jwtSecret, claims, 120);
            claims.AddClaim(new Claim("token", token));

            //var refreshToken = AuthUtil.GenerateRefreshToken();

            // Save tokens to DB
            //user.Token = token;


            JwtTokenReturn jwt1 = new JwtTokenReturn
            {
                FullName = user.FirstName + " " + user.LastName,
                Role = role.Name,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                LoginTime = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                Token = token,
                //ClinicName = user.ClinicName,
                //ClinicAddress = user.ClinicAddress,
                //HomeLink = user.HomeLink
            };

            return jwt1;

            //await repository.Update(user);
            //return new TokenDTO { Token = token, RefreshToken = refreshToken };
            //throw new NotImplementedException("h");
        }

        public async Task<(User user, string message)> RegisterUser(UserDto model)
        {

            var userExists = await _repository.Query().FirstOrDefaultAsync(r => r.Email == model.UsernameOrEmail);
            if (userExists != null) return (user: null, message: $"User {userExists.Email} exists already!");
            AuthUtil.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userDetails = new User
            {
                CreatedAt = DateTime.UtcNow,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = model.UsernameOrEmail,
            };

            var newuser = await _repository.Insert(userDetails);

            return (user: newuser, message: "User created successfully.");
        }
    }
    public interface IAuthManager
    {
        Task<TokenDto> LogUserIn(UserDto model);
        Task<JwtTokenReturn> SignInUserAsync(Employee user);
        Task<(User user, string message)> RegisterUser(UserDto model);

        Task<ServiceResponse<AuthResponseDTO>> Login(UserDto credential);
    }
}
