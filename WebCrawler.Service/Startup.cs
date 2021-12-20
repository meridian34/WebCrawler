using System;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.ConsoleApplication.Services;

namespace WebCrawler.ConsoleApplication
{
    public class Startup
    {
        private static readonly IServiceProvider _provider;
        static Startup()
        {
            var service = new ServiceCollection();
            service.AddWebCrawler();
            service.AddTransient<ConsoleService>();
            service.AddTransient<CrawlerService>();
            _provider = service.BuildServiceProvider();
        }

        public static CrawlerService GetWebCrawler => _provider.GetService<CrawlerService>();
    }
}
