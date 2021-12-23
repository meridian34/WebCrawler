using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebCrawler.EntityFramework.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddWebCrawlerRepository(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddEfRepository<ApplicationDbContext>(options);
            services.AddScoped<DataProvider>();
         
            return services;
        }
    }
}
