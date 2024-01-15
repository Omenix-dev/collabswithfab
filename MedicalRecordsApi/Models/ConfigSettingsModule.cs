using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalRecordsApi.Models
{
    public static class ConfigSettingsModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Does not apply")]
        public static void AddConfigSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTConfig>(configuration.GetSection(JWTConfig.ConfigName));
        }
    }
}
