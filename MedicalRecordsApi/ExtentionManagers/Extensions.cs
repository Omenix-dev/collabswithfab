
using MedicalRecordsApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisitionManagerApi.ExtentionManagers
{
    public static class Extensions
    {
        public static void AddAppAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = new JwtSettings();

            config.Bind(nameof(jwtSettings), jwtSettings);

            // Register jwt settings into IoC
            services.AddSingleton(jwtSettings);

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        RequireExpirationTime=false,
                        ValidateLifetime=true

                    };
                });
        }
    }
}
