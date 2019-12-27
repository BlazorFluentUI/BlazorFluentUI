using Microsoft.Extensions.DependencyInjection;

namespace BlazorFabric
{
    public static class ServiceExtension
    {
        public static void AddBlazorFabric(this IServiceCollection services)
        {
            services.AddSingleton<IComponentStyleSheets, ComponentStyleSheets>();
        }
    }
}