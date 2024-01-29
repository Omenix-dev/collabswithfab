using MedicalRecordsApi.Helpers;
using MedicalRecordsApi.Services.Abstract.PatientInterfaces;
using MedicalRecordsApi.Services.Common;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsApi.Services.Implementation.PatientServices;
using MedicalRecordsApi.Utils;
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
            //Helpers
            services.AddSingleton<IClaimHelper, ClaimHelper>();

			//Doctors
			services.AddScoped<IPatientService, PatientService>();

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
            services.AddScoped<IGenericService<Lab>, GenericService<Lab>>();
            services.AddAutoMapper(typeof(PatientProfileMapper));
        }
    }
}