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
            var htmlCrawler = new HtmlCrawler( new RequestService(), new HtmlParser(new UrlValidatorService(),new LinkConvertorService()), new LinkConvertorService());
            var sitemapCrawler = new SitemapCrawler( new RequestService(), new SitemapParser(new UrlValidatorService()), new LinkConvertorService());
            var webCrawler = new WebCrawlerService(htmlCrawler, sitemapCrawler, new UrlValidatorService(), new RequestService());
            var consoleService = new ConsoleService();
            var crawlerService = new CrawlerService(consoleService, webCrawler);
            crawlerService.Run();
        }
    }
}