using System;
using System.Net;
using System.Threading.Tasks;

using ApiKeyAuthApp.Models;

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

namespace ApiKeyAuthApp
{
    public class ApiKeyAuthHttpTrigger
    {
        private readonly ILogger<ApiKeyAuthHttpTrigger> _logger;

        public ApiKeyAuthHttpTrigger(ILogger<ApiKeyAuthHttpTrigger> log)
        {
            this._logger = log ?? throw new ArgumentNullException(nameof(log));
        }

        [FunctionName(nameof(ApiKeyAuthHttpTrigger.GetGreeting))]

        // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
        [OpenApiOperation(operationId: "Greeting", tags: new[] { "greeting" })]
        [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "Ocp-Apim-Subscription-Key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GreetingResponse), Description = "The OK response")]
        // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

        public async Task<IActionResult> GetGreeting(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "greeting")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var message = string.IsNullOrWhiteSpace(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var response = new GreetingResponse()
            {
                Message = message,
            };

            var result = new OkObjectResult(response);

            return await Task.FromResult(result);
        }
    }
}