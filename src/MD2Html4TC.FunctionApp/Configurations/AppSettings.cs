using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace MD2Html4TC.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the app settings entity.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="StorageAccountSettings"/> object.
        /// </summary>
        public virtual StorageAccountSettings StorageAccount { get; set; } = new StorageAccountSettings();

        /// <summary>
        /// Gets or sets the <see cref="ApplicationInsightsSettings"/> object.
        /// </summary>
        public virtual ApplicationInsightsSettings ApplicationInsights { get; set; } = new ApplicationInsightsSettings();

        /// <summary>
        /// Gets the <see cref="OpenApiSettings"/> object.
        /// </summary>
        public virtual OpenApiSettings OpenApi { get; } = new OpenApiSettings();

        /// <summary>
        /// Gets the <see cref="MsGraphSettings"/> object.
        /// </summary>
        public virtual MsGraphSettings MsGraph { get; } = new MsGraphSettings();

        /// <summary>
        /// Gets the <see cref="ConverterSettings"/> object.
        /// </summary>
        public virtual ConverterSettings Converter { get; } = new ConverterSettings();
    }

    /// <summary>
    /// This represents the app settings entity for Azure Storage Account.
    /// </summary>
    public class StorageAccountSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for Azure Application Insights.
    /// </summary>
    public class ApplicationInsightsSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for OpenAPI.
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// Gets the settings name.
        /// </summary>
        public const string Name = "OpenApi";

        /// <summary>
        /// Gets or sets the <see cref="OpenApiVersionType"/> value.
        /// </summary>
        public virtual OpenApiVersionType Version { get; set; }

        /// <summary>
        /// Gets or sets the OpenAPI document version.
        /// </summary>
        public virtual string DocVersion { get; set; }

        /// <summary>
        /// Gets or sets the OpenAPI document title.
        /// </summary>
        public virtual string DocTitle { get; set; }

        /// <summary>
        /// Gets or sets the OpenAPI document description
        /// </summary>
        public virtual string DocDescription { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for Microsoft Graph.
    /// </summary>
    public class MsGraphSettings
    {
        /// <summary>
        /// Gets the settings name.
        /// </summary>
        public const string Name = "MsGraph";

        /// <summary>
        /// Gets or sets the URL for login.
        /// </summary>
        public virtual string LoginUrl { get; set; }

        /// <summary>
        /// Gets or sets the tenant ID.
        /// </summary>
        public virtual string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public virtual string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public virtual string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the MS Graph API URL.
        /// </summary>
        public virtual string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public virtual string BaseUrl { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for the converter.
    /// </summary>
    public class ConverterSettings
    {
        /// <summary>
        /// Gets the settings name.
        /// </summary>
        public const string Name = "Converter";

        /// <summary>
        /// Gets or sets the list of HTML tags to add extra blank paragraph.
        /// </summary>
        public virtual IEnumerable<string> HtmlTags { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for the emoji.
    /// </summary>
    public class EmojiSettings
    {
        /// <summary>
        /// Gets the settings name.
        /// </summary>
        public const string Name = "Emoji";

        /// <summary>
        /// Gets or sets the list of HTML tags to add extra blank paragraph.
        /// </summary>
        public virtual string ApiUrl { get; set; }
    }
}