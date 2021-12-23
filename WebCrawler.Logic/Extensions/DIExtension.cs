using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Logic.Services;

namespace WebCrawler.Logic.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddWebCrawlerLogic(this IServiceCollection services)
        {
            services.AddScoped<DataStorageService>();

            return services;
        }
    }
}
