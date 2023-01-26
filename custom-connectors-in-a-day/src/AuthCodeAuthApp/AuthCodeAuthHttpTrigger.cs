using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AuthCodeAuthApp.Models;

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

namespace AuthCodeAuthApp
{
    public class AuthCodeAuthHttpTrigger
    {
        private readonly GraphSettings _settings;
        private readonly HttpClient _http;
        private readonly ILogger<AuthCodeAuthHttpTrigger> _logger;

        public AuthCodeAuthHttpTrigger(GraphSettings settings, IHttpClientFactory factory, ILogger<AuthCodeAuthHttpTrigger> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("profile");
            this._logger = log.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(AuthCodeAuthHttpTrigger.GetProfile))]
        [OpenApiOperation(operationId: "Profile", tags: new[] { "profile" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GraphUser), Description = "The OK response")]
        public async Task<IActionResult> GetProfile(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "profile")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            // Pass through the bearer token details to the downstream API
            var auth = req.Headers["Authorization"].ToString();
            this._http.DefaultRequestHeaders.Clear();
            this._http.DefaultRequestHeaders.Add("Authorization", auth);

            var result = await this._http.GetStringAsync($"https://graph.microsoft.com/v1.0/{this._settings.Endpoint.Trim('/')}");
            var response = JsonConvert.DeserializeObject<GraphUser>(result);

            return new OkObjectResult(response);
        }
    }
}