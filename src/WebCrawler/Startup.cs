using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Services;
using WebCrawler.Services.Abstractions;

namespace WebCrawler
{
    public class Startup
    {
        private static readonly IServiceCollection _services;
        private static readonly IServiceProvider _provider;

        static Startup()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("config.json");
            var configuration = builder.Build();
            var contentTypesHtml = configuration.GetSection("contentTypesForHtml").Get<string[]>();
            var contentTypesXml = configuration.GetSection("contentTypesForXml").Get<string[]>();
            var delay = configuration.GetSection("delay").Get<int>();
            var maxConcarency = configuration.GetSection("maxConcarency").Get<int>();

            _services = new ServiceCollection();
            _services.AddTransient<IComparerService, ComparerService>();
            _services.AddTransient<IHtmlDocumentService, HtmlDocumentService>();
            _services.AddTransient<IUrlsRepositoryService, UrlsRepositoryService>();
            _services.AddTransient<ISitemapDataService, SitemapDataService>();
            _services.AddTransient<IWebHandlerFactory>((serviceProvider) =>
            {
                return new WebHandlerFactory(
                    maxConcarency,
                    delay,
                    contentTypesHtml,
                    contentTypesXml);
            });
            _services.AddTransient<ISiteScanService, SiteScanService>();
            _services.AddTransient<ISiteMapService, SiteMapService>();
            _services.AddTransient<IWebCrawlerService, WebCrawlerService>();
            _services.BuildServiceProvider();
            _provider = _services.BuildServiceProvider();
        }

        public static IWebCrawlerService GetWebCrawler => _provider.GetService<IWebCrawlerService>();
    }
}
