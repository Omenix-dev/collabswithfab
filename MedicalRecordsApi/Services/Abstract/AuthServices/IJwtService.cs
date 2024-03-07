using MedicalRecordsApi.Utils;
using MedicalRecordsData.Entities.AuthEntity;

namespace MedicalRecordsApi.Services.Abstract.AuthServices
{
    public interface IJwtService
    {
        Jwt GetJwt(User user);
        string GenerateRefreshToken();
    }
}
