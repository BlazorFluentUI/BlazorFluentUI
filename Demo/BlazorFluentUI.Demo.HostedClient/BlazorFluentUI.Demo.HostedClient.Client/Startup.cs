using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using BlazorFluentUI.Demo.Shared;

namespace BlazorFluentUI.Demo.HostedClient.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazorFluentUI();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLocalTimeZone();
            app.AddComponent<App>("app");
            app.AddComponent<BFUGlobalRules>("#staticcs");
        }
    }
}
