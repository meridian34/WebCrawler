using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;

namespace WebCrawler
{
    public static class DIExtension
    {
        public static IServiceCollection AddWebCrawler(this IServiceCollection services)
        {   
            services.AddTransient<UrlValidatorService>();
            services.AddTransient<LinkConvertorService>();
            services.AddTransient<SitemapParser>();
            services.AddTransient<HtmlParser>();
            services.AddTransient<HttpClientService>();
            services.AddTransient<WebRequestService>();
            services.AddTransient<SitemapCrawler>();
            services.AddTransient<HtmlCrawler>();
            services.AddTransient<WebCrawlerService>();

            return services;
        }
    }
}
