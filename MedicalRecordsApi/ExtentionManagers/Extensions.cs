
using MedicalRecordsApi.Models.DTO;
using MedicalRecordsApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RequisitionManagerApi.ExtentionManagers
{
    public static class Extensions
    {
        public static void AddAppAuthentication(this IServiceCollection services, IConfiguration config)
        {
            // Add Authentication
            services.AddAuthentication(opt =>
            {

                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(tok =>
                {
                    tok.SaveToken = true;
                    tok.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Key"])),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                    };

                    tok.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            // Override the default behavior
                            context.HandleResponse();

                            // Create your custom response here
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var responseObj = new AppExceptionResponse(new UnauthorizedException("Unauthorized"));
                            return context.Response.WriteAsync(JsonSerializer.Serialize(responseObj));
                        }
                    };
                });
        }
    }
}