using MedicalRecordsApi.Models;
using System;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Common.Interfaces
{
    public interface IHttpService
    {
        Task<ApiResponse<string>> MakeHttpRequestAsync(Uri requestUri, string payload, string authToken, AuthType authType, CustomHttpMethod httpMethod);
    }
}
