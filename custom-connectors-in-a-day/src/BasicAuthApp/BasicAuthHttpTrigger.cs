using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using BasicAuthApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

namespace BasicAuthApp
{
    public class BasicAuthHttpTrigger
    {
        private readonly AtlassianSettings _settings;
        private readonly HttpClient _http;
        private readonly ILogger<BasicAuthHttpTrigger> _logger;

        public BasicAuthHttpTrigger(AtlassianSettings settings, IHttpClientFactory factory, ILogger<BasicAuthHttpTrigger> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("profile");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(BasicAuthHttpTrigger.GetProfile))]
        [OpenApiOperation(operationId: "Profile", tags: new[] { "profile" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AtlassianUser), Description = "The OK response")]
        public async Task<IActionResult> GetProfile(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "profile")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            // Pass through the basic auth details to the downstream API
            var auth = req.Headers["Authorization"].ToString();
            this._http.DefaultRequestHeaders.Clear();
            this._http.DefaultRequestHeaders.Add("Authorization", auth);

            var result = await this._http.GetStringAsync($"https://{this._settings.InstanceName}.atlassian.net/wiki/rest/api/{this._settings.Endpoint.Trim('/')}");
            var response = JsonConvert.DeserializeObject<AtlassianUser>(result);

            return new OkObjectResult(response);
        }
    }
}