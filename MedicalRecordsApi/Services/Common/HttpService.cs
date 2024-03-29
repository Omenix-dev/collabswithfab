﻿using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using MedicalRecordsApi.Models;
using MedicalRecordsApi.Services.Common.Interfaces;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace MedicalRecordsApi.Services.Common
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpService> _logger;
        private readonly IConfiguration _configuration;

        private const string ZohoTokenName = "ZohoToken";

        public HttpService(IHttpClientFactory clientFactory, ILogger<HttpService> logger,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> MakeHttpRequestAsync(Uri requestUri, string payload, string authToken, AuthType authType, CustomHttpMethod httpMethod)
        {
            HttpClient httpClient = _clientFactory.CreateClient();

            if (authType == AuthType.Basic)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            }
            else if(authType == AuthType.Bearer) 
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", authToken);
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(httpMethod.ToString()),
                RequestUri = requestUri
            };

            if (!string.IsNullOrEmpty(payload))
            {
                StringContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = content;
            }

            HttpResponseMessage httpResponse = new HttpResponseMessage
            {
                ReasonPhrase = "Network Error",
                StatusCode = HttpStatusCode.GatewayTimeout
            };

            string responseString = string.Empty;

            try
            {
                httpResponse = await httpClient.SendAsync(httpRequestMessage);
                responseString = await httpResponse.Content.ReadAsStringAsync();

                httpResponse.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                httpResponse.ReasonPhrase += $". {ex.Message}";
                _logger.LogError(ex.ToString());
            }

            ApiResponse<string> responseDto = new ApiResponse<string>
            {
                IsSuccess = httpResponse.IsSuccessStatusCode,
                Data = responseString,
                Status = (int)httpResponse.StatusCode,
                Reason = httpResponse.ReasonPhrase
            };

            httpResponse.Dispose();
            httpRequestMessage.Dispose();
            httpClient.Dispose();

            return responseDto;
        }

    }

}
