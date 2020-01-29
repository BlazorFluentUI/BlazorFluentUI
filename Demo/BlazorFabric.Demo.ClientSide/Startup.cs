using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorFabric.Demo.ClientSide
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazorFabric();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLocalTimeZone();
            app.AddComponent<App>("app");
            app.AddComponent<GlobalRules>("#staticcs");
        }
    }
}
