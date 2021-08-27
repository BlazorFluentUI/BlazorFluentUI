using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFluentUI
{
    public static class ServiceExtension
    {
        public static void AddBlazorFluentUI(this IServiceCollection services)
        {
            services.AddScoped<ObjectIDGenerator>();
            services.AddScoped<IComponentStyle, ComponentStyle>();
            services.AddScoped<ThemeProvider>();
            services.AddScoped<ScopedStatics>();
            services.AddScoped<LayerHostService>();
        }
    }
}