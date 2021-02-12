using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentUI.Demo.ClientSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //builder.RootComponents.Add<GlobalRules>("#staticcs");

            builder.RootComponents.Add<FluentUI.Demo.Shared.App>("#app");

            builder.Services.AddFluentUI();
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            await builder.Build().RunAsync();
        }

        //public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //    BlazorWebAssemblyHost.CreateDefaultBuilder()
        //        .UseBlazorStartup<Startup>();
    }
}