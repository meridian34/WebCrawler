using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Services.Services;

namespace WebCrawler.Services.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddWebCrawlerServices(this IServiceCollection services)
        {
            services.AddScoped<DataStorageService>();
            services.AddScoped<DataProcessingService>();
            return services;
        }
    }
}
