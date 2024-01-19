using MedicalRecordsApi.Managers.Auth;
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
using System.Threading.Tasks;

namespace MedicalRecordsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
	{
		private IAuthManager _auth;

		private readonly MedicalRecordDbContext _Context;

		public AuthController(MedicalRecordDbContext context, IAuthManager auth)
		{
			_Context = context;
			_auth = auth;
		}

		//api/<AuthController>
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(UserDTO user)
		{
			var response = new APIResponse();
			response.ApiMessage = $"Error: User credentials in correct!";
			response.StatusCode = "01";
			response.Result = null;
			var jwt = await _auth.LogUserIn(user);
			if (jwt != null)
			{
				response.Result = jwt;
				response.StatusCode = "00";
				response.ApiMessage = "Successful Login!";
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPost("signin")]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn(int id)
		{
			var response = new APIResponse();
			response.ApiMessage = $"Error: User credentials in correct!";
			response.StatusCode = "01";
			response.Result = null;

			var link = await _Context.Resources.SingleOrDefaultAsync();
			var user_auth = await _Context.Employees.Where(s => s.Id == id).FirstOrDefaultAsync();
			if (user_auth != null)
			{
				User data = new User
				{
					Id = user_auth.Id,
					FirstName = user_auth.FirstName,
					LastName = user_auth.LastName,
					Role = "",
					Email = user_auth.Email,
					HomeLink = link.HomeLink,

					ClinicName = await _Context.Clinics.Where(x => x.Id == user_auth.ClinicId).Select(m => m.Name).FirstOrDefaultAsync(),
					ClinicAddress = await _Context.Clinics.Where(x => x.Id == user_auth.ClinicId).Select(m => m.Location).FirstOrDefaultAsync(),

				};

				var jwt = _auth.SignInUser(data);
				if (jwt != null)
				{
					response.Result = jwt;
					response.StatusCode = "00";
					response.ApiMessage = "Successful Login!";
					return Ok(response);
				}
			}
			return Ok(response);

		}

		[HttpPost("authorization")]
		[AllowAnonymous]
		public async Task<IActionResult> MainLogin(string urltoken)
		{
			var response = new APIResponse();
			response.ApiMessage = $"Error: User credentials in correct!";
			response.StatusCode = "01";
			response.Result = null;


			var user_auth = await _Context.Employees.Where(s => s.AuthenticationToken == urltoken).FirstOrDefaultAsync();
			if (user_auth != null)
			{
				var role = "";
				var URole = (from ur1 in _Context.UserRoles
							 join r1 in _Context.Roles on ur1.RoleId equals r1.Id
							 where ur1.UserId == user_auth.Id && ur1.Status == 1
							 select new
							 {
								 Id = ur1.RoleId,
								 Name = r1.Name
							 }).FirstOrDefault();

				if (URole != null) { if (URole.Id > 0) { role = URole.Name; } }

				var link = await _Context.Resources.SingleOrDefaultAsync();

				User data = new User
				{
					Id = user_auth.Id,
					FirstName = user_auth.FirstName,
					LastName = user_auth.LastName,
					Role = role,
					Email = user_auth.Email,
					Picture = user_auth.ProfilePicture,
					HomeLink = link.HomeLink,
					ClinicName = await _Context.Clinics.Where(x => x.Id == user_auth.ClinicId).Select(m => m.Name).FirstOrDefaultAsync(),
					ClinicAddress = await _Context.Clinics.Where(x => x.Id == user_auth.ClinicId).Select(m => m.Location).FirstOrDefaultAsync(),
				};


				var jwt = _auth.SignInUser(data);

				if (jwt != null)
				{
					response.Result = jwt;
					response.StatusCode = "00";
					response.ApiMessage = "Successful Login!";
					return Ok(response);
				}
			}
			return Ok(response);

		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register(UserDTO userdto)
		{
			var response = new APIResponse();
			response.StatusCode = "01";
			response.Result = null;
			var (user, message) = await _auth.RegisterUser(userdto); ;
			if (user != null)
			{
				response.Result = user;
				response.StatusCode = "00";
				response.ApiMessage = message;
				return Ok(response);
			}
			response.ApiMessage = message;

			return BadRequest(response);
		}

	}
}
