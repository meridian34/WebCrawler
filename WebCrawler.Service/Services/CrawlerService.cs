using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Run()
        {
            var url = _consoleService.ReadLine();
            var res = _webCrawlerService.GetLinks(new Uri(url));
            PrintSitemapUniqueLink(res.Where(x => x.IsSitemap == true && x.IsCrawler == false));
            PrintSiteScanUniqueLink(res.Where(x => x.IsSitemap == false && x.IsCrawler == true));
            var res2 = _webCrawlerService.GetPerfomanceDataCollection(res);
            PrintResultTime(res2);
            PrintCrawlerCount(res.Where(x => x.IsCrawler == true).Count());
            PrintSitemapCount(res.Where(x => x.IsSitemap == true).Count());
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

        private void PrintCrawlerCount(int countCrawlerResults)
        {
            _consoleService.WriteLine($"Urls(html documents) found after crawling a website: {countCrawlerResults}");
        }

        private void PrintSitemapCount( int countSitemapResults)
        {
            _consoleService.WriteLine($"Urls found in sitemap: {countSitemapResults}");
        }
    }
}
