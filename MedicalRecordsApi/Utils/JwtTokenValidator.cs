using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MedicalRecordsApi.Utils
{
    public class JwtTokenValidator
    {
        public JwtTokenReturn RetrieveToken(ClaimsIdentity identity)
        {
            JwtTokenReturn tokenObejct = new JwtTokenReturn();
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;

                if (int.TryParse((claims.Where(p => p.Type == "id").FirstOrDefault()?.Value), out int id))
                {
                    if (id > 0)
                    {
                        tokenObejct.UserId = id;
                        tokenObejct.FullName = claims.Where(p => p.Type == "Name").FirstOrDefault()?.Value;
                        tokenObejct.Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;
                        tokenObejct.Role = claims.Where(p => p.Type == "Role").FirstOrDefault()?.Value;
                     //   token_obejct.MdaId = claims.Where(p => p.Type == "MdaId").FirstOrDefault()?.Value;
                        //var name = claims.Where(p => p.Type == "userid").FirstOrDefault()?.Value;
                    }
                }
            }

            return tokenObejct;
        }
    }
}
