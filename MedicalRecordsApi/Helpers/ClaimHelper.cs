using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MedicalRecordsApi.Helpers
{
    public class ClaimHelper : IClaimHelper
    {
        private readonly IHttpContextAccessor _context;

        public ClaimHelper(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetPublicUser()
        {
            return GetClaimValue("UserId");
        }

        public string GetUserEmail()
        {
            return GetClaimValue("Email");
        }

        public string GetUserPhoneNumber()
        {
            return GetClaimValue("PhoneNumber");
        }

        public string GetClaimValue(string claimType) //e.g LoginID or ClaimType.Role
        {
            ClaimsIdentity identity = _context.HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return null;
            }

            Claim claim = identity.FindFirst(claimType);

            if (claim == null)
            {
                return null;
            }

            return string.IsNullOrEmpty(claim.Value) ? null : claim.Value;
        }
    }
}
