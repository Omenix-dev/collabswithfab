namespace MedicalRecordsApi.Models
{
    public class JwtConfig
    {
        public const string ConfigName = nameof(JwtConfig);

        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
	}
}
