using MedicalRecordsApi.Helpers;
using MedicalRecordsApi.Services.Common;
using MedicalRecordsApi.Services.Common.Interfaces;
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

            //Helpers
            services.AddSingleton<IClaimHelper, ClaimHelper>();

        }
    }
}