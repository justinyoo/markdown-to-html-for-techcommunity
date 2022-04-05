using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Aliencube.AzureFunctions.Extensions.Common;

using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Examples;
using MD2Html4TC.FunctionApp.Extensions;
using MD2Html4TC.FunctionApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

namespace MD2Html4TC.FunctionApp
{
    /// <summary>
    /// This represents the HTTP trigger entity that manages user details with Azure Active Directory.
    /// </summary>
    public class UserDetailsHttpTrigger
    {
        private readonly MsGraphSettings _settings;
        private readonly ILogger<UserDetailsHttpTrigger> _logger;

        public UserDetailsHttpTrigger(MsGraphSettings settings, ILogger<UserDetailsHttpTrigger> logger)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._logger = logger.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(UserDetailsHttpTrigger.GetUserDetailsAsync))]
        [OpenApiOperation(operationId: "getUser", tags: new[] { "registration" }, Summary = "Get user details", Description = "This endpoint gets the user details.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(LoggedInUser), Example = typeof(LoggedInUserExample), Summary = "Response payload including the logged-in user details.", Description = "Response payload that includes the logged-in user details.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "This indicates the logged-in user doesn't exist.")]
        public async Task<IActionResult> GetUserDetailsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.GET, Route = "users/get")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");
            var headers = await req.To<IHeaderDictionary>(SourceFrom.Header).ConfigureAwait(false);
            if (headers.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var base64encoded = headers.TryGetValue("x-ms-client-principal", out var value) ? (string)value : default;
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64encoded));
            var principal = JsonConvert.DeserializeObject<ClientPrincipal>(json);

            var client = await this.GetGraphClientAsync().ConfigureAwait(false);

            var users = await client.Users.Request().GetAsync().ConfigureAwait(false);
            var user = users.SingleOrDefault(p => p.Mail == principal.UserDetails);
            if (user.IsNullOrDefault())
            {
                return new NotFoundResult();
            }

            var loggedInUser = new LoggedInUser(user);

            return new OkObjectResult(loggedInUser);
        }

        private async Task<GraphServiceClient> GetGraphClientAsync()
        {
            var baseUri = $"{this._settings.ApiUrl.TrimEnd('/')}/{this._settings.BaseUrl}";
            var provider = new DelegateAuthenticationProvider(async p =>
            {
                var accessToken = await this.GetAccessTokenAsync().ConfigureAwait(false);
                p.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            });
            var client = new GraphServiceClient(baseUri, provider);

            return await Task.FromResult(client).ConfigureAwait(false);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var scopes = new[] { $"{this._settings.ApiUrl.TrimEnd('/')}/.default" };
            var options = this._settings.ToConfidentialClientApplicationOptions();
            var authority = $"{options.Instance.TrimEnd('/')}/{options.TenantId}";

            var app = ConfidentialClientApplicationBuilder
                          .Create(options.ClientId)
                          .WithClientSecret(options.ClientSecret)
                          .WithAuthority(authority)
                          .Build();

            var result = await app.AcquireTokenForClient(scopes)
                                  .ExecuteAsync()
                                  .ConfigureAwait(false);
            var accessToken = result.AccessToken;

            return accessToken;
        }
    }
}