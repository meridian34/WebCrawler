using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface IWebCrawlerService
    {
        public event Action<IReadOnlyCollection<HttpScanResult>> SendingSitemapUniqueResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingScanUniqueResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingSitemapResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingScanResults;

        public event Action<IReadOnlyCollection<HttpScanResult>> SendingAllSortedResults;

        public IReadOnlyCollection<HttpScanResult> GetSitemapUniqueResults();

        public IReadOnlyCollection<HttpScanResult> GetScanUniqueResults();

        public IReadOnlyCollection<HttpScanResult> GetSitemapResults();

        public IReadOnlyCollection<HttpScanResult> GetScanResults();

        public IReadOnlyCollection<HttpScanResult> GetAllSortedResults();

        public Task RunCrowler(string url);
    }
}
