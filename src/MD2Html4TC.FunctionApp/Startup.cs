using System.Text.RegularExpressions;

using MD2Html4TC.FunctionApp.Configurations;
using MD2Html4TC.FunctionApp.Triggers;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MD2Html4TC.FunctionApp.Startup))]

namespace MD2Html4TC.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureAppSettings(builder.Services);
            this.ConfigureClients(builder.Services);
            this.ConfigureServices(builder.Services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();

            var regex = new Regex("\\<pre\\>\\<code class=\"language\\-(.+)\"\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            services.AddSingleton(regex);
        }

        private void ConfigureClients(IServiceCollection services)
        {
            services.AddHttpClient<ConvertHttpTrigger>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
        }
    }
}