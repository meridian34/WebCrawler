using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class WebCrawlingResult
    {
        public IReadOnlyCollection<HttpScanResult> SitemapResults { get; set; }

        public IReadOnlyCollection<HttpScanResult> ScanResults { get; set; }

        public IReadOnlyCollection<HttpScanResult> SitemapUniqueResults { get; set; }

        public IReadOnlyCollection<HttpScanResult> ScanUniqueResults { get; set; }

        public IReadOnlyCollection<HttpScanResult> AllResults { get; set; }
    }
}
