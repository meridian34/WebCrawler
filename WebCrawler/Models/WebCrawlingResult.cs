using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class WebCrawlingResult
    {
        public IReadOnlyCollection<CrawlResult> SitemapResults { get; set; }

        public IReadOnlyCollection<CrawlResult> ScanResults { get; set; }

        public IReadOnlyCollection<CrawlResult> SitemapUniqueResults { get; set; }

        public IReadOnlyCollection<CrawlResult> ScanUniqueResults { get; set; }

        public IReadOnlyCollection<CrawlResult> AllResults { get; set; }
    }
}
