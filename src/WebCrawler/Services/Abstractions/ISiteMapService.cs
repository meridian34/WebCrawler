using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface ISiteMapService
    {
        public Task<IReadOnlyCollection<HttpScanResult>> MapAsync(string sitemapXmlUrl);
    }
}
