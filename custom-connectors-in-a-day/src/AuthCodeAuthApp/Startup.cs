using System;

using AuthCodeAuthApp.Models;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
// ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️

[assembly: FunctionsStartup(typeof(AuthCodeAuthApp.Startup))]

namespace AuthCodeAuthApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureAppSettings(builder.Services);
            ConfigureHttpClient(builder.Services);
        }

        private static void ConfigureAppSettings(IServiceCollection services)
        {
            // ⬇️⬇️⬇️ 아래의 코드 주석을 막아주세요 ⬇️⬇️⬇️
            // var settings = new GraphSettings();
            // services.BuildServiceProvider()
            //         .GetService<IConfiguration>()
            //         .GetSection(GraphSettings.Name)
            //         .Bind(settings);
            // services.AddSingleton(settings);
            // ⬆️⬆️⬆️ 위의 코드 주석을 막아주세요 ⬆️⬆️⬆️

            // ⬇️⬇️⬇️ 아래의 코드 주석을 풀어주세요 ⬇️⬇️⬇️
            var settings = services.BuildServiceProvider()
                                   .GetService<IConfiguration>()
                                   .Get<GraphSettings>(GraphSettings.Name);
            services.AddSingleton(settings);

            var options = new DefaultOpenApiConfigurationOptions()
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info = new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "API AuthN'd by Authorization Code Auth",
                    Description = "This is the API authN'd by Authorization Code Auth."
                }
            };

            var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
            if (codespaces)
            {
                options.IncludeRequestingHostName = false;
            }

            services.AddSingleton<IOpenApiConfigurationOptions>(options);
            // ⬆️⬆️⬆️ 위의 코드 주석을 풀어주세요 ⬆️⬆️⬆️
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("profile");
        }
    }
}