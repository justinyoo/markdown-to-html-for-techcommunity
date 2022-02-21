using System;
using System.Collections.Generic;
using System.Linq;

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
        public virtual string ConnectionString { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.StorageAccountKey) ?? string.Empty;
    }

    /// <summary>
    /// This represents the app settings entity for Azure Application Insights.
    /// </summary>
    public class ApplicationInsightsSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.AppInsightsKey) ?? string.Empty;
    }

    /// <summary>
    /// This represents the app settings entity for OpenAPI.
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiVersionType"/> value.
        /// </summary>
        public virtual OpenApiVersionType Version { get; set; } = Enum.TryParse<OpenApiVersionType>(
                                                                       Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiVersionKey), ignoreCase: true, out var result)
                                                                     ? result
                                                                     : OpenApiConfigurationOptions.DefaultVersion();

        /// <summary>
        /// Gets or sets the OpenAPI document version.
        /// </summary>
        public virtual string DocumentVersion { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocVersionKey) ?? OpenApiConfigurationOptions.DefaultDocVersion();

        /// <summary>
        /// Gets or sets the OpenAPI document title.
        /// </summary>
        public virtual string DocumentTitle { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocTitleKey) ?? OpenApiConfigurationOptions.DefaultDocTitle(typeof(AppSettings));

        /// <summary>
        /// Gets or sets the OpenAPI document description
        /// </summary>
        public virtual string DocumentDescription { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocDescriptionKey) ?? OpenApiConfigurationOptions.DefaultDocDescription(typeof(AppSettings));
    }

    /// <summary>
    /// This represents the app settings entity for Microsoft Graph.
    /// </summary>
    public class MsGraphSettings
    {
        /// <summary>
        /// Gets or sets the URI for login.
        /// </summary>
        public virtual string LoginUri { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphLoginUriKey);

        /// <summary>
        /// Gets or sets the tenant ID.
        /// </summary>
        public virtual string TenantId { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphTenantIdKey);

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public virtual string ClientId { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphClientIdKey);

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public virtual string ClientSecret { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphClientSecretKey);

        /// <summary>
        /// Gets or sets the MS Graph API URI.
        /// </summary>
        public virtual string ApiUri { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphApiUriKey);

        /// <summary>
        /// Gets or sets the base URI.
        /// </summary>
        public virtual string BaseUri { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.MsGraphBaseUriKey);
    }

    /// <summary>
    /// This represents the app settings entity for the converter.
    /// </summary>
    public class ConverterSettings
    {
        /// <summary>
        /// Gets or sets the list of HTML tags to add extra blank paragraph.
        /// </summary>
        public virtual IEnumerable<string> HtmlTags { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.ConverterHtmlTagsKey)
                                                                        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                                                        .Select(p => p.Trim());
    }
}