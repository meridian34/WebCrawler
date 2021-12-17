using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using WebCrawler.ConsoleApplication.Services;
using System.Threading.Tasks;

namespace WebCrawler.ConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var htmlCrawler = new HtmlCrawler( new WebRequestService( new HttpClientService()), new HtmlParser(new UrlValidatorService(),new LinkConvertorService()), new LinkConvertorService());
            var sitemapCrawler = new SitemapCrawler( new WebRequestService( new HttpClientService()), new SitemapParser(new UrlValidatorService()), new LinkConvertorService());
            var webCrawler = new WebCrawlerService(htmlCrawler, sitemapCrawler, new UrlValidatorService(), new WebRequestService(new HttpClientService()));
            var consoleService = new ConsoleService();
            var crawlerService = new CrawlerService(consoleService, webCrawler);
            await crawlerService.RunAsync();
        }
    }
}