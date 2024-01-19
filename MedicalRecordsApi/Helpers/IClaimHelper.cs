namespace MedicalRecordsApi.Helpers
{
    public interface IClaimHelper
    {
        string GetClaimValue(string claimType);

        string GetPublicUser();

        string GetUserEmail();

        string GetUserPhoneNumber();
    }
}
