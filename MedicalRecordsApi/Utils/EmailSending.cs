using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Utils
{
    public class EmailSending
    {
        public class Email
        {
            public string Subject { get; set; }
            public string Message { get; set; }
            public string Toaddy { get; set; }
            public string Cctoaddy { get; set; }
            public string Bcctoaddy { get; set; }
            public string User { get; set; }
        }

        public class EmailResponse
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public EmailResponse Save(Email email)
        {
            var link = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("EmailLink");

            RestClient restClient = new RestClient(link.Value);

            //Creating Json object
            JObject objectBody = new JObject();
            objectBody.Add("Subject", email.Subject);
            objectBody.Add("Message", email.Message);
            objectBody.Add("Toaddy", email.Toaddy);
            objectBody.Add("Cctoaddy", email.Cctoaddy);
            objectBody.Add("Bcctoaddy", email.Bcctoaddy);
            objectBody.Add("User", email.User);

            RestRequest restRequest = new RestRequest(Method.POST);

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", objectBody, ParameterType.RequestBody);
            IRestResponse restResponse = restClient.Execute(restRequest);

            var response2 = restResponse.Content;

            var data = JsonConvert.DeserializeObject<EmailResponse>(response2);

            return data;
        }
    }
}
