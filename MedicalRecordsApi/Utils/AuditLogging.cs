using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_structure.Utils
{
    public class AuditLogging
    {
        public class Audit
        {
            public int userid { get; set; }
            public string module { get; set; }
            public string message { get; set; }
        }
        public void Save(Audit audit)
        {
            var link = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AuditLink");

            RestClient restClient = new RestClient(link.Value);

            //Creating Json object
            JObject objectBody = new JObject();
            objectBody.Add("userid", audit.userid);
            objectBody.Add("module", audit.module);
            objectBody.Add("message", audit.message);

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
