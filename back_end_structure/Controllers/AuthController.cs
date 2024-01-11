using back_end_structure.Data;
using back_end_structure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_structure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MedicalRecordsContext _context;

        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthController> logger;
        private static ILogger staticLogger = Utils.ApplicationLogging.CreateLogger<AuthController>();

        public AuthController(MedicalRecordsContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            this.logger = logger;
        }

        public class auth
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<object> SignIn(auth data)
        {
            Token token = new Token(_configuration);
            var user_auth = await _context.Employees.Where(s => s.Email == data.email).FirstOrDefaultAsync();
            var res = await _context.Resources.Take(1).SingleOrDefaultAsync();
            if (user_auth != null)
            {
                return Ok(new
                {
                    Id = user_auth.Id,
                    FirstName = user_auth.FirstName,
                    LastName = user_auth.LastName,
                    Role = "Request tester",
                    Email = user_auth.Email,
                    Picture = user_auth.ProfilePicture,
                    token = token.BuildToken(user_auth.Id.ToString(), "new"),
                    homeurl = res.HomeLink
                });
            }

            return Ok(new
            {
                code = 0,
                message = "Record not found"
            });
        }

        [HttpPost("authorization")]
        [AllowAnonymous]
        public async Task<object> MainLogin(string urltoken)
        {
            Token token = new Token(_configuration);
            var user_auth = await _context.Employees.Where(s => s.AuthenticationToken == urltoken).FirstOrDefaultAsync();
            var res = await _context.Resources.Take(1).SingleOrDefaultAsync();
            if (user_auth != null)
            {
                var role = "";
                var URole = (from ur1 in _context.Roles 
                             
                             where ur1.Id == user_auth.RoleId
                             select new
                             {
                                 Id = ur1.Id,
                                 Name = ur1.Name
                             }).FirstOrDefault();

                if (URole != null) { if (URole.Id > 0) { role = URole.Name; } }

                //dynamic obj = new System.Dynamic.ExpandoObject();

                return Ok(new
                {
                    Id = user_auth.Id,
                    FirstName = user_auth.FirstName,
                    LastName = user_auth.LastName,
                    Role = role,
                    Email = user_auth.Email,
                    Picture = user_auth.ProfilePicture,
                    token = token.BuildToken(user_auth.Id.ToString(), $"{user_auth.FirstName} {user_auth.LastName}"),
                    homeurl = res.HomeLink
                });

            }
            return Ok(new
            {
                code = 0,
                message = "Record not found"
            });
        }
    }
}
