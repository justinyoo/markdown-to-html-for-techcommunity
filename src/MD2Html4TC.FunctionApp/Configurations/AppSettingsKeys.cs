namespace MD2Html4TC.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the constants entity for app settings.
    /// </summary>
    public static class AppSettingsKeys
    {
        /// <summary>
        /// Identifies Azure Storage Account connection string key.
        /// </summary>
        public const string StorageAccountKey = "AzureWebJobsStorage";

        /// <summary>
        /// Identifies Azure Application Insights connectionstring key.
        /// </summary>
        public const string AppInsightsKey = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        /// <summary>
        /// Identifies the OpenApi key.
        /// </summary>
        public const string OpenApiKey = "OpenApi";

        /// <summary>
        /// Identifies the OpenAPI version key.
        /// </summary>
        public const string OpenApiVersionKey = "OpenApi__Version";

        /// <summary>
        /// Identifies the OpenAPI document version key.
        /// </summary>
        public const string OpenApiDocVersionKey = "OpenApi__DocVersion";

        /// <summary>
        /// Identifies the OpenAPI document title key.
        /// </summary>
        public const string OpenApiDocTitleKey = "OpenApi__DocTitle";

        /// <summary>
        /// Identifies the OpenAPI document description key.
        /// </summary>
        public const string OpenApiDocDescriptionKey = "OpenApi__DocDescription";

        /// <summary>
        /// Identifies the login URI key to to Microsoft Graph.
        /// </summary>
        public const string MsGraphLoginUriKey = "MsGraph__LoginUri";

        /// <summary>
        /// Identifies the tenant ID key to to Microsoft Graph.
        /// </summary>
        public const string MsGraphTenantIdKey = "MsGraph__TenantId";

        /// <summary>
        /// Identifies the client ID key to to Microsoft Graph.
        /// </summary>
        public const string MsGraphClientIdKey = "MsGraph__ClientId";

        /// <summary>
        /// Identifies the client secret key to to Microsoft Graph.
        /// </summary>
        public const string MsGraphClientSecretKey = "MsGraph__ClientSecret";

        /// <summary>
        /// Identifies the API URI of the Microsoft Graph API.
        /// </summary>
        public const string MsGraphApiUriKey = "MsGraph__ApiUri";

        /// <summary>
        /// Identifies the base URI of the Microsoft Graph API.
        /// </summary>
        public const string MsGraphBaseUriKey = "MsGraph__BaseUri";

        /// <summary>
        /// Identifies the list of HTML tags to add extra paragraph, delimited by a comma.
        /// </summary>
        public const string ConverterHtmlTagsKey = "Converter__HtmlTags";
    }
}