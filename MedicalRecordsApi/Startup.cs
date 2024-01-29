using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsApi.Models;
using MedicalRecordsRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MedicalRecordsApi.Managers.Auth;
using RequisitionManagerApi.ExtentionManagers;
using MedicalRecordsRepository.Interfaces;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsApi.Utils;
using MedicalRecordsApi.Services;

namespace MedicalRecordsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
		{
			var connstr = Configuration.GetConnectionString("MedicalRecords");
			services.AddControllers();

			services.AddDbContextPool<MedicalRecordDbContext>(
				 dbContextOptions => dbContextOptions
					 .UseSqlServer(connstr)
			        // TODO: Please remove when develop is over.
			        //.EnableSensitiveDataLogging()
			        //.EnableDetailedErrors()
			);

			services.AddAppAuthentication(Configuration);

			//Services
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddServices();

            services.AddCoreRepository();
			services.AddConfigSettings(Configuration);

			services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Greenzone Medical Records API",
                    Version = "v1",
                    Description = "Greenzone Medical Records API with endpoints for Medical Records."
				});

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

            });

			#region Setup Managers

			services.AddScoped<IAuthManager, AuthManager>();

			#endregion

			services.AddHttpContextAccessor();

			services.AddCors(options => options.AddPolicy("AllowFromAll", builder => builder.WithMethods("GET", "POST", "DELETE", "PUT").AllowAnyOrigin().AllowAnyHeader()));
			
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			
            services.AddControllers(config =>
			{
				var policy = new AuthorizationPolicyBuilder()
								 .RequireAuthenticatedUser()
								 .Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseDeveloperExceptionPage();

			//app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("AllowFromAll");
			// loggerFactory.AddLog4Net(); // << Add this line

			// Utils.ApplicationLogging.LoggerFactory = loggerFactory;

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", " Medical Records Module V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
