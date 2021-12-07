using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface IWebCrawlerService
    {
        public IReadOnlyCollection<HttpScanResult> GetSitemapUniqueResults();

        public IReadOnlyCollection<HttpScanResult> GetScanUniqueResults();

        public IReadOnlyCollection<HttpScanResult> GetSitemapResults();

        public IReadOnlyCollection<HttpScanResult> GetScanResults();

        public IReadOnlyCollection<HttpScanResult> GetAllSortedResults();

        public Task RunCrawler(string url);
    }
}
