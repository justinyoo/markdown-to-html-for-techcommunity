using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Helpers;
using MD2Html4TC.FunctionApp.Resolvers;
using MD2Html4TC.FunctionApp.Services;
using MD2Html4TC.FunctionApp.Triggers;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MD2Html4TC.FunctionApp.Startup))]

namespace MD2Html4TC.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                   .AddEnvironmentVariables();

            base.ConfigureAppConfiguration(builder);
        }

        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureAppSettings(builder.Services);
            this.ConfigureClients(builder.Services);
            this.ConfigureResolvers(builder.Services);
            this.ConfigureHelpers(builder.Services);
            this.ConfigureServices(builder.Services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            var openapi = config.Get<OpenApiSettings>(OpenApiSettings.Name);
            services.AddSingleton(openapi);

            var msgraph = config.Get<MsGraphSettings>(MsGraphSettings.Name);
            services.AddSingleton(msgraph);

            var converter = config.Get<ConverterSettings>(ConverterSettings.Name);
            services.AddSingleton(converter);

            var emoji = config.Get<EmojiSettings>(EmojiSettings.Name);
            services.AddSingleton(emoji);
        }

        private void ConfigureClients(IServiceCollection services)
        {
            services.AddHttpClient<ConvertHttpTrigger>();
        }

        private void ConfigureResolvers(IServiceCollection services)
        {
            services.AddSingleton<IRegexResolver, TechCommunityRegexResolver>();
            services.AddSingleton<IRegexResolver, EmojiRegexResolver>();
        }

        private void ConfigureHelpers(IServiceCollection services)
        {
            services.AddTransient<IMarkdownHelper, MarkdownHelper>();
            services.AddTransient<IEmojiHelper, EmojiHelper>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMarkdownService, MarkdownService>();
        }
    }
}