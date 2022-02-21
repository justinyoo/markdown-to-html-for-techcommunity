using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using Markdig;

using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MD2Html4TC.FunctionApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity that converts markdown to HTML.
    /// </summary>
    public class ConvertHttpTrigger
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<ConvertHttpTrigger> _logger;
        private readonly HttpClient _httpClient;
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertHttpTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="logger"><see cref="ILogger"/> instance.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        /// <param name="regex"><see cref="Regex"/> instance.</param>
        public ConvertHttpTrigger(AppSettings settings, ILogger<ConvertHttpTrigger> logger, HttpClient httpClient, Regex regex)
        {
            this._appSettings = settings.ThrowIfNullOrDefault();
            this._logger = logger.ThrowIfNullOrDefault();
            this._httpClient = httpClient.ThrowIfNullOrDefault();
            this._regex = regex.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the conversion method from markdown to HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance that contains the markdown string.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the HTML string converted from markdown.</returns>
        [FunctionName(nameof(ConvertHttpTrigger.ConvertAsync))]
        [OpenApiOperation(operationId: "convert", tags: new[] { "md2html" }, Summary = "Convert Markdown to HTML", Description = "Convert Markdown to HTML", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: ContentTypes.PlainText, bodyType: typeof(string), Required = true, Description = "Markdown contents")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.PlainText, bodyType: typeof(string), Summary = "HTML contents converted from Markdown", Description = "HTML contents")]
        public async Task<IActionResult> ConvertAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.POST, Route = "convert/md/to/html")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var md = default(string);
            using (var reader = new StreamReader(req.Body))
            {
                md = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseEmojiAndSmiley().UseYamlFrontMatter().Build();
            var html = Markdown.ToHtml(md, pipeline);
            html = this._regex.Replace(html, "<li-code lang=\"$1\">")
                              .Replace("</code></pre>", "</li-code>")

                              .AddEmptyParagraph(this._appSettings.Converter.HtmlTags, this._appSettings.Converter.HtmlTags)
                              ;

            var result = new ContentResult()
            {
                Content = html,
                ContentType = "text/plain",
                StatusCode = (int)HttpStatusCode.OK
            };

            return result;
        }
    }
}