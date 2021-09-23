using System.Collections.Generic;
using System.Linq;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class ComparerService : IComparerService
    {
        public IReadOnlyCollection<HttpScanResult> GetUniqueSitemapResult(IReadOnlyCollection<HttpScanResult> sitemapResults, IReadOnlyCollection<HttpScanResult> scanResults)
        {
            return sitemapResults.Where(x => scanResults.Where(y => y.Url == x.Url).FirstOrDefault() == null).ToList();
        }

        public IReadOnlyCollection<HttpScanResult> GetUniqueScanResult(IReadOnlyCollection<HttpScanResult> scanResults, IReadOnlyCollection<HttpScanResult> sitemapResults)
        {
            return scanResults.Where(x => sitemapResults.Where(y => y.Url == x.Url).FirstOrDefault() == null).ToList();
        }
    }
}
