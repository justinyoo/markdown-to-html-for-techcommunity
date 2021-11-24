using System.Text.Json.Serialization;

namespace MD2Html4TC.BlazorApp.Models
{
    /// <summary>
    /// This represents the entity for authentication details.
    /// </summary>
    public class AuthenticationDetails
    {
        /// <summary>
        /// Gets or sets the <see cref="ClientPrincipal"/> instance.
        /// </summary>
        [JsonPropertyName("clientPrincipal")]
        public ClientPrincipal ClientPrincipal { get; set; }
    }
}
