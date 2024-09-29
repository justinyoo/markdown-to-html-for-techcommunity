using System.Text.RegularExpressions;

using MD2Html.FunctionApp.Configs;
using MD2Html.Services;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var host = new HostBuilder()
    // .ConfigureFunctionsWebApplication()
    .ConfigureFunctionsWebApplication(worker => worker.UseNewtonsoftJson())
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            var options = new OpenApiConfigurationOptions()
            {
                Info = new OpenApiInfo()
                {
                    Version = "v1.0.0",
                    Title = "Markdown to HTML converter",
                    Description = "This provides a converter API from markdown to HTML.",
                },
                OpenApiVersion = OpenApiVersionType.V3,
            };

            return options;
        });

        services.AddSingleton<HtmlSettings>(sp =>
        {
            var config = sp.GetService<IConfiguration>();

            var settings = config!.GetSection(HtmlSettings.Name).Get<HtmlSettings>();

            return settings!;
        });

        services.AddSingleton<Regex>(sp =>
        {
            var regex = new Regex("\\<pre\\>\\<code class=\"language\\-(.+)\"\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex;
        });

        services.AddScoped<IConverterService, ConverterService>();
    })
    .Build();

await host.RunAsync();
