namespace MedicalRecordsApi.Models
{
    public enum AuthType
    {
        Bearer = 1,
        Basic,
        ZohoOAuth
    }

    public enum CustomHttpMethod
    {
        Get = 1,
        Post,
        Put,
        Delete,
        Option
    }
}
