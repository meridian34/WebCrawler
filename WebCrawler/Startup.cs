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

            _services = new ServiceCollection();
            _services.AddTransient<HtmlDocumentService>();
            _services.AddTransient<WebHandlerFactory>();
            _services.AddTransient<SiteScanService>();
            _services.AddTransient<SiteMapService>();
            _services.AddTransient<WebCrawlerService>();
            _services.BuildServiceProvider();
            _provider = _services.BuildServiceProvider();
        }

        public static IWebCrawlerService GetWebCrawler => _provider.GetService<IWebCrawlerService>();
    }
}
