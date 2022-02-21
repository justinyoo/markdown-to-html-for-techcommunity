using MD2Html4TC.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace MD2Html4TC.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="LoggedInUser"/>.
    /// </summary>
    public class LoggedInUserExample : OpenApiExample<LoggedInUser>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<LoggedInUser> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "loggedin",
                    new LoggedInUser()
                    {
                        Upn = "natasha.romanoff@contoso.com",
                        Email = "natasha.romanoff@contoso.com",
                        DisplayName = "Natasha Romanoff",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}