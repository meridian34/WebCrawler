using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCrawler.ConsoleApplication.Services;
using WebCrawler.EntityFramework;
using WebCrawler.Services.Extensions;

namespace WebCrawler.ConsoleApplication
{
    public class Startup
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddWebCrawler();
                services.AddTransient<ConsoleService>();
                services.AddTransient<CrawlerService>();
                services.AddEfRepository<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("connectionString")));
                services.AddWebCrawlerServices();
                
            });

    }
}
