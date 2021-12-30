using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebCrawler.ConsoleApplication.Services;

namespace WebCrawler.ConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Startup.CreateHostBuilder(args).Build();
            var service = host.Services.GetRequiredService<CrawlerService>();
            await service.RunAsync();
        }
    }
}