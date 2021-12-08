using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface ISiteScanService
    {
        public Task<IReadOnlyCollection<HttpScanResult>> ScanSiteAsync(string url);
    }
}
