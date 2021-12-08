using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services.Abstractions
{
    public interface IWebHandlerService
    {
        public Task<HttpScanResult> ScanUrlAsync(string url);
        Task<HttpScanResult> ScanUrlAsync(HttpScanResult result);
        Task<IReadOnlyCollection<HttpScanResult>> ScanUrlConcurencyAsync(IReadOnlyCollection<HttpScanResult> urls);
    }
}
