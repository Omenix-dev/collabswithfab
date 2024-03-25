using MedicalRecordsApi.Helpers;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.AuthServices;
using MedicalRecordsApi.Services.Abstract.CustomerEngagementInterfaces;
using MedicalRecordsApi.Services.Abstract.DashBoardInterfaces;
using MedicalRecordsApi.Services.Abstract.EmployeeInterfaces;
using MedicalRecordsApi.Services.Abstract.FacilityInterfaces;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsApi.Services.Implementation.AuthServices;
using MedicalRecordsApi.Services.Implementation.CustomerEngagementServices;
using MedicalRecordsApi.Services.Implementation.DashBoardServices;
using MedicalRecordsApi.Services.Implementation.EmployeeServices;
using MedicalRecordsApi.Services.Implementation.FacilityServices;
using MedicalRecordsApi.Services.Implementation.PatientServices;
using MedicalRecordsApi.Services.Profiles;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalRecordsApi.Services
{
    public static class ServicesModule
    {
        //IMPORTANT NOTE: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2#service-lifetimes-and-registration-options
        //For DB calls, use Scoped
        //For HttpClients, use Singleton : https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8

        public static void AddServices(this IServiceCollection services)
        {
            //Common
            services.AddSingleton<IHttpService, HttpService>();
            services.AddHttpClient();

            //Employee
            services.AddScoped<IEmployeeService, EmployeeService>();
            
            //Helpers
            services.AddSingleton<IClaimHelper, ClaimHelper>();

			//Patients
			services.AddScoped<IPatientService, PatientService>();

            //Facility
            services.AddScoped<IFacilityService, FacilityService>();

            //DashBoard
            services.AddScoped<IDashBoardService, DashBoardService>();

            //Customer Engagement
            services.AddScoped<ICustomerEngagementService, CustomerEngagementService>();

            //JWT
            services.AddScoped<IJwtService, JwtService>();

            //Generic class instantiations
            services.AddScoped<IGenericService<Patient>, GenericService<Patient>>();
			services.AddScoped<IGenericService<Contact>, GenericService<Contact>>();
			services.AddScoped<IGenericService<EmergencyContact>, GenericService<EmergencyContact>>();
			services.AddScoped<IGenericService<Immunization>, GenericService<Immunization>>();
			services.AddScoped<IGenericService<ImmunizationDocument>, GenericService<ImmunizationDocument>>();
			services.AddScoped<IGenericService<MedicalRecord>, GenericService<MedicalRecord>>();
			services.AddScoped<IGenericService<Medication>, GenericService<Medication>>();
			services.AddScoped<IGenericService<PatientReferrer>, GenericService<PatientReferrer>>();
			services.AddScoped<IGenericService<Treatment>, GenericService<Treatment>>();
            services.AddScoped<IGenericService<Visit>, GenericService<Visit>>();
            services.AddScoped<IGenericService<LabRequest>, GenericService<LabRequest>>();
            services.AddScoped<IGenericService<User>, GenericService<User>>();
            services.AddScoped<IGenericService<Bed>, GenericService<Bed>>();
            services.AddScoped<IGenericService<AssignPatientBed>, GenericService<AssignPatientBed>>();
            services.AddScoped<IGenericService<Employee>, GenericService<Employee>>();
            services.AddScoped<IGenericService<PatientLabReport>, GenericService<PatientLabReport>>();
            services.AddScoped<IGenericService<EmployeePrivilegeAccess>, GenericService<EmployeePrivilegeAccess>>();
            services.AddScoped<IGenericService<ReadCustomerFeedbackDto>, GenericService<ReadCustomerFeedbackDto>>();
            services.AddScoped<IGenericService<AssignedPatientsDto>, GenericService<AssignedPatientsDto>>();
            services.AddScoped<IGenericService<PatientAssignmentHistory>, GenericService<PatientAssignmentHistory>>();


            services.AddAutoMapper(typeof(PatientProfileMapper));
            services.AddAutoMapper(typeof(MedicalRecordsMapProfile));
        }
    }
}