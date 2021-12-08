using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class WebCrawlingResult
    {
        public IReadOnlyCollection<HttpScanResult> SitemapResults;
        public IReadOnlyCollection<HttpScanResult> ScanResults;
        public IReadOnlyCollection<HttpScanResult> SitemapUniqueResults;
        public IReadOnlyCollection<HttpScanResult> ScanUniqueResults;
        public IReadOnlyCollection<HttpScanResult> AllResults;
    }
}
