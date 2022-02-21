using System;
using System.Net.Http;
using System.Threading.Tasks;

using MD2Html4TC.BlazorApp.Helpers;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MD2Html4TC.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
                            .AddScoped<IAuthHelper, AuthHelper>();

            await builder.Build().RunAsync();
        }
    }
}