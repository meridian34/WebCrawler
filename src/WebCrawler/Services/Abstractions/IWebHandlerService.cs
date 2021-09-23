using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface IWebHandlerService
    {
        public Task<HttpScanResult> ScanUrlAsync(string url);

        public Task<IReadOnlyCollection<HttpScanResult>> ScanUrlConcurencyAsync(IReadOnlyCollection<string> urls);
    }
}
