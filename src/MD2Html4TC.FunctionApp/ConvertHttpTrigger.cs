using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Markdig;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MD2Html4TC.FunctionApp
{
    public static class ConvertHttpTrigger
    {
        private static Regex regex = new Regex("\\<pre\\>\\<code class=\"language\\-(.+)\"\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        [FunctionName("ConvertHttpTrigger")]
        [OpenApiOperation(operationId: "convert", tags: new[] { "md2html" }, Summary = "Convert Markdown to HTML", Description = "Convert Markdown to HTML", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "API Key through the request header")]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(string), Required = true, Description = "Markdown contents")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "HTML contents converted from Markdown", Description = "HTML contents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "convert/md/to/html")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var md = default(string);
            using (var reader = new StreamReader(req.Body))
            {
                md = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseEmojiAndSmiley().UseYamlFrontMatter().Build();
            var html = Markdown.ToHtml(md, pipeline);
            html = regex.Replace(html, "<li-code lang=\"$1\">")
                        .Replace("</code></pre>", "</li-code>");

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
