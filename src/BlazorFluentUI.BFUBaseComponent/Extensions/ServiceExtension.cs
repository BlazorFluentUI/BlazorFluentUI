using Microsoft.Extensions.DependencyInjection;

namespace BlazorFluentUI
{
    public static class ServiceExtension
    {
        public static void AddBlazorFluentUI(this IServiceCollection services)
        {
            services.AddScoped<IComponentStyle, ComponentStyle>();
            services.AddScoped<ThemeProvider>();
            services.AddScoped<ScopedStatics>();
        }
    }
}