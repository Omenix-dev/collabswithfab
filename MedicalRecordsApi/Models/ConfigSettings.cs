namespace MedicalRecordsApi.Models
{
    public class JWTConfig
    {
        public const string ConfigName = nameof(JWTConfig);

        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
	}
}
