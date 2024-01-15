namespace MedicalRecordsApi.Helpers
{
    public interface IClaimHelper
    {
        string GetClaimValue(string claimType);

        string GetPublicUser();

        string GetInternalLoggedInUser();

        string GetLoggedInAgent();

        string GetLoggedInPartner();

        string GetUserEmail();

        string GetUserPhoneNumber();
    }
}
