using System;
using System.Collections.Generic;
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

        public void Run(string url)
        {
            PrintSitemapUniqueLink(_webCrawlerService.GetUniqueSitemapLinks(url));
            PrintSiteScanUniqueLink(_webCrawlerService.GetUniqueHtmlLinks(url));
            PrintTimeResult(_webCrawlerService.GetUniqueCrawlingResult(url));
            PrintCount(_webCrawlerService.GetHtmlLinksCount(url), _webCrawlerService.GetSitemapLinksCount(url));
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

        private void PrintTimeResult(IEnumerable<PerfomanceData> results)
        {
            _consoleService.WriteLine($"Timings");

            foreach (var i in results)
            {
                _consoleService.WriteLine($"{i.Url}");
                _consoleService.WriteLine($"{i.ElapsedMilliseconds}");
            }

            _consoleService.WriteLine("\n");
        }

        private void PrintCount(int countScanResults, int countSitemapResults)
        {
            _consoleService.WriteLine($"Urls(html documents) found after crawling a website: {countScanResults}");
            _consoleService.WriteLine($"Urls found in sitemap: {countSitemapResults}");
        }
    }
}
