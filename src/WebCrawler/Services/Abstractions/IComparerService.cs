using System.Collections.Generic;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface IComparerService
    {
        public IReadOnlyCollection<HttpScanResult> GetUniqueSitemapResult(IReadOnlyCollection<HttpScanResult> sitemapResults, IReadOnlyCollection<HttpScanResult> scanResults);

        public IReadOnlyCollection<HttpScanResult> GetUniqueScanResult(IReadOnlyCollection<HttpScanResult> scanResults, IReadOnlyCollection<HttpScanResult> sitemapResults);
    }
}
