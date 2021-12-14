using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using WebCrawler.ConsoleApplication.Services;

namespace WebCrawler.ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var htmlCrawler = new HtmlCrawler(new UrlValidatorService(), new RequestService(), new HtmlParser(), new LinkConvertorService());
            var sitemapCrawler = new SitemapCrawler(new UrlValidatorService(), new RequestService(), new SitemapParser(), new LinkConvertorService());
            var webCrawler = new WebCrawlerService(htmlCrawler, sitemapCrawler, new UrlValidatorService(), new RequestService());
            var consoleService = new ConsoleService();
            var crawlerService = new CrawlerService(consoleService, webCrawler);
            crawlerService.Run();
        }
    }
}