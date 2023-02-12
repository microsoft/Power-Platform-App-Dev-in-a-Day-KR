using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AuthCodeAuthApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

// ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;
// ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

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
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._http = factory == null ? throw new ArgumentNullException(nameof(factory)) : factory.CreateClient("profile");
            this._logger = log ?? throw new ArgumentNullException(nameof(log));
        }

        [FunctionName(nameof(AuthCodeAuthHttpTrigger.GetProfile))]

        // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
        [OpenApiOperation(operationId: "Profile", tags: new[] { "profile" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GraphUser), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "The bad request response")]
        // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

        public async Task<IActionResult> GetProfile(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "profile")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            // Pass through the bearer token details to the downstream API
            if (req.Headers.ContainsKey("Authorization") == false)
            {
                return new BadRequestObjectResult("Authorization header is missing");
            }

            var auth = req.Headers["Authorization"].ToString();
            this._http.DefaultRequestHeaders.Clear();
            this._http.DefaultRequestHeaders.Add("Authorization", auth);

            var result = await this._http.GetStringAsync($"https://graph.microsoft.com/v1.0/{this._settings.Endpoint.Trim('/')}");
            var response = JsonConvert.DeserializeObject<GraphUser>(result);

            return new OkObjectResult(response);
        }
    }
}