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
    public class ErrorLogging
    {
        public class Error
        {
            public int userid { get; set; }
            public string module { get; set; }
            public string part { get; set; }
            public string message { get; set; }
        }

        public void Save(Error error)
        {
            var link = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ErrorLink");

            RestClient restClient = new RestClient(link.Value);

            //Creating Json object
            JObject objectBody = new JObject();
            objectBody.Add("userid", error.userid);
            objectBody.Add("module", error.module);
            objectBody.Add("message", error.message);
            objectBody.Add("part", error.part);

            RestRequest restRequest = new RestRequest(Method.POST);

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", objectBody, ParameterType.RequestBody);
            IRestResponse restResponse = restClient.Execute(restRequest);

            var response2 = restResponse.Content;

            //var data = JsonConvert.DeserializeObject<AccResponse>(response2);

            //return data;
        }
    }
}
