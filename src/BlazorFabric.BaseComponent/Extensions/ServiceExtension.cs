using Microsoft.Extensions.DependencyInjection;

namespace BlazorFabric
{
    public static class ServiceExtension
    {
        public static void AddBlazorFabric(this IServiceCollection services)
        {
            services.AddScoped<IComponentStyle, ComponentStyle>();
            services.AddScoped<ThemeProvider>();
            services.AddScoped<ScopedStatics>();
        }
    }
}