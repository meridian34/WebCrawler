using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services;

namespace WebCrawler.ConsoleApplication.Services
{
    public class CrawlerService
    {
        private readonly ConsoleService _consoleService;
        private readonly WebCrawlerService _webCrawlerService;

        public CrawlerService(ConsoleService consoleService, WebCrawlerService webCrawlerService)
        {
            _consoleService = consoleService;
            _webCrawlerService = webCrawlerService;
        }

        public async Task RunAsync()
        {
            var url = _consoleService.ReadLine();

            var links = await _webCrawlerService.GetLinksAsync(new Uri(url));
            var uniqueSitemapLinks = links.Where(x => x.FromSitemap == true && x.FromHtml == false);
            var uniqueHtmlLinks = links.Where(x => x.FromSitemap == false && x.FromHtml == true);

            PrintSitemapUniqueLink(uniqueSitemapLinks);
            PrintSiteScanUniqueLink(uniqueHtmlLinks);

            var perfomanceData = await _webCrawlerService.GetPerfomanceDataCollectionAsync(links);

            PrintResultTime(perfomanceData);
            PrintCount(links.Where(x => x.FromHtml == true).Count(), links.Where(x => x.FromSitemap == true).Count());
        }

        private void PrintSitemapUniqueLink(IEnumerable<Link> results)
        {
            PrintResult(results, "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
        }

        private void PrintSiteScanUniqueLink(IEnumerable<Link> results)
        {
            PrintResult(results, "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
        }

        private void PrintResult(IEnumerable<Link> results, string headerMessage)
        {
            _consoleService.WriteLine($"{headerMessage}");

            foreach (var i in results)
            {
                _consoleService.WriteLine($"{i.Url}");
            }

            _consoleService.WriteLine("\n");
        }

        private void PrintResultTime(IEnumerable<PerfomanceData> results)
        {
            _consoleService.WriteLine($"Timings");

            foreach (var i in results)
            {
                _consoleService.WriteLine($"{i.Url}");
                _consoleService.WriteLine($"{i.ElapsedMilliseconds}");
            }

            _consoleService.WriteLine("\n");
        }

        private void PrintCount(int countCrawlerResults, int countSitemapResults)
        {
            _consoleService.WriteLine($"Urls(html documents) found after crawling a website: {countCrawlerResults}");
            _consoleService.WriteLine($"Urls found in sitemap: {countSitemapResults}");
        }

    }
}
