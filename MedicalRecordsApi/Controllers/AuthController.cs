using MedicalRecordsApi.Managers.Auth;
using MedicalRecordsApi.Models.DTO;
using MedicalRecordsApi.Services;
using MedicalRecordsApi.Utils;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsRepository.DTO.AuthDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
	{
		private readonly IAuthManager _auth;

		private readonly MedicalRecordDbContext _context;

		public AuthController(MedicalRecordDbContext context, IAuthManager auth)
		{
			_context = context;
			_auth = auth;
		}


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserDto credential)
        {
            ServiceResponse<AuthResponseDTO> result = await _auth.Login(credential);

            return result.FormatResponse();
        }


        ////api/<AuthController>
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> LoginTest(UserDto user)
        //{
        //    var response = new ApiResponse();
        //    response.ApiMessage = $"Error: User credentials in correct!";
        //    response.StatusCode = "01";
        //    response.Result = null;
        //    var jwt = await _auth.LogUserIn(user);
        //    if (jwt != null)
        //    {
        //        response.Result = jwt;
        //        response.StatusCode = "00";
        //        response.ApiMessage = "Successful Login!";
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}

        //[HttpPost("signin")]
        //[AllowAnonymous]
        //public async Task<IActionResult> SignIn(int id)
        //{
        //    var response = new ApiResponse();
        //    response.ApiMessage = $"Error: User credentials in correct!";
        //    response.StatusCode = "01";
        //    response.Result = null;

        //    //var link = await _context.Resources.SingleOrDefaultAsync();
        //    var userAuth = await _context.Employees.Where(s => s.Id == id).FirstOrDefaultAsync();
        //    if (userAuth != null)
        //    {
        //        //User data = new User
        //        //{
        //        //    Id = userAuth.Id,
        //        //    FirstName = userAuth.FirstName,
        //        //    LastName = userAuth.LastName,
        //        //    Role = "",
        //        //    Email = userAuth.Email,
        //        //    //HomeLink = link.HomeLink,

        //        //    //ClinicName = await _context.Clinics.Where(x => x.Id == userAuth.ClinicId).Select(m => m.Name).FirstOrDefaultAsync(),
        //        //    //ClinicAddress = await _context.Clinics.Where(x => x.Id == userAuth.ClinicId).Select(m => m.Location).FirstOrDefaultAsync(),
        //        //};

        //        var jwt = await _auth.SignInUserAsync(userAuth);
        //        if (jwt != null)
        //        {
        //            response.Result = jwt;
        //            response.StatusCode = "00";
        //            response.ApiMessage = "Successful Login!";
        //            return Ok(response);
        //        }
        //    }
        //    return Ok(response);

        //}

        ////[HttpPost("authorization")]
        ////[AllowAnonymous]
        ////public async Task<IActionResult> MainLogin(string urltoken)
        ////{
        ////    var response = new ApiResponse();
        ////    response.ApiMessage = $"Error: User credentials in correct!";
        ////    response.StatusCode = "01";
        ////    response.Result = null;


        ////    var userAuth = await _context.Employees.Where(s => s.AuthenticationToken == urltoken).FirstOrDefaultAsync();
        ////    if (userAuth != null)
        ////    {
        ////        var role = "";
        ////        var uRole = (from ur1 in _context.UserRoles
        ////                     join r1 in _context.Roles on ur1.RoleId equals r1.Id
        ////                     where ur1.UserId == userAuth.Id && ur1.Status == 1
        ////                     select new
        ////                     {
        ////                         Id = ur1.RoleId,
        ////                         Name = r1.Name
        ////                     }).FirstOrDefault();

        ////        if (uRole != null) { if (uRole.Id > 0) { role = uRole.Name; } }

        ////        var link = await _context.Resources.SingleOrDefaultAsync();

        ////        User data = new User
        ////        {
        ////            Id = userAuth.Id,
        ////            FirstName = userAuth.FirstName,
        ////            LastName = userAuth.LastName,
        ////            Role = role,
        ////            Email = userAuth.Email,
        ////            Picture = userAuth.ProfilePicture,
        ////            HomeLink = link.HomeLink,
        ////            ClinicName = await _context.Clinics.Where(x => x.Id == userAuth.ClinicId).Select(m => m.Name).FirstOrDefaultAsync(),
        ////            ClinicAddress = await _context.Clinics.Where(x => x.Id == userAuth.ClinicId).Select(m => m.Location).FirstOrDefaultAsync(),
        ////        };


        ////        var jwt = _auth.SignInUser(data);

        ////        if (jwt != null)
        ////        {
        ////            response.Result = jwt;
        ////            response.StatusCode = "00";
        ////            response.ApiMessage = "Successful Login!";
        ////            return Ok(response);
        ////        }
        ////    }
        ////    return Ok(response);

        ////}

        //[HttpPost("register")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(UserDto userdto)
        //{
        //    var response = new ApiResponse();
        //    response.StatusCode = "01";
        //    response.Result = null;
        //    var (user, message) = await _auth.RegisterUser(userdto); ;
        //    if (user != null)
        //    {
        //        response.Result = user;
        //        response.StatusCode = "00";
        //        response.ApiMessage = message;
        //        return Ok(response);
        //    }
        //    response.ApiMessage = message;

        //    return BadRequest(response);
        //}

    }
}
