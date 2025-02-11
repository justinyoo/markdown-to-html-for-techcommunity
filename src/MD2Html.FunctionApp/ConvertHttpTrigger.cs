using System.Net;
using System.Text.RegularExpressions;

using MD2Html.FunctionApp.Configs;
using MD2Html.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MD2Html.FunctionApp;

/// <summary>
/// This represents the HTTP trigger entity.
/// </summary>
/// <param name="settings"><see cref="HtmlSettings"/> instance.</param>
/// <param name="service"><see cref="IConverterService"/> instance.</param>
/// <param name="logger"><see cref="ILogger{T}"/> instance.</param>
public class ConvertHttpTrigger(HtmlSettings settings, IConverterService service, ILogger<ConvertHttpTrigger> logger)
{
    private readonly HtmlSettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    private readonly IConverterService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly ILogger<ConvertHttpTrigger> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Invokes the conversion method from markdown to HTML.
    /// </summary>
    /// <param name="req"><see cref="HttpRequest"/> instance that contains the markdown string.</param>
    /// <returns>Returns the HTML string converted from markdown.</returns>
    [Function("ConvertHttpTrigger")]
    [OpenApiOperation(operationId: "convert", tags: [ "md2html" ], Summary = "Convert Markdown to HTML", Description = "Convert Markdown to HTML", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "API Key through the request header")]
    [OpenApiParameter(name: "tc", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Summary = "Value indicating whether to convert it for Tech Community or not", Description = "This value indicates whether the markdown document is converted for Tech Community or not")]
    [OpenApiParameter(name: "p", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Summary = "Value indicating whether to inject extra empty paragraph in between paragraphs or not", Description = "This value indicates whether the extra empty paragraph is injected in between paragraphs or not")]
    [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(string), Required = true, Description = "Markdown contents")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "HTML contents converted from Markdown", Description = "HTML contents")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "text/plain", bodyType: typeof(string), Summary = "Something went wrong", Description = "Something went wrong on the server-side")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "convert/md/to/html")] HttpRequest req)
    {
        this._logger.LogInformation("C# HTTP trigger function processed a request.");

        var result = new ContentResult()
        {
            ContentType = "text/plain",
        };

        try
        {
            var md = default(string);
            using (var reader = new StreamReader(req.Body))
            md = await reader.ReadToEndAsync();

            var tc = bool.TryParse(req.Query["tc"], out var tcParsed) && tcParsed;
            var p = bool.TryParse(req.Query["p"], out var pParsed) && pParsed;
            var htmlTags = this._settings.Tags!
                                        .Split([ "," ], StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => p.Trim());

            var html = await this._service.ConvertToHtmlAsync(md, tc, p, htmlTags);
            result.Content = html;
            result.StatusCode = (int)HttpStatusCode.OK;

            return result;
        }
        catch (Exception ex)
        {
            result.Content = ex.Message;
            result.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result;
        }
    }
}
