using System;
using System.Threading.Tasks;
using WebCrawler.Services.Models;

namespace WebCrawler.Services.Services
{
    public class DataProcessingService
    {
        private readonly DataStorageService _storage;
        private readonly WebCrawlerService _webCrawler;

        public DataProcessingService(DataStorageService storage, WebCrawlerService webCrawler)
        {
            _storage = storage;
            _webCrawler = webCrawler;
        }

        public virtual async Task StartCrawlingSite(Uri url)
        {
            var links = await _webCrawler.GetLinksAsync(url);
            var perfomanceData = await _webCrawler.GetPerfomanceDataCollectionAsync(links);
            await _storage.SaveAsync(url.OriginalString, links, perfomanceData);
        } 

        public virtual async Task<TestsPage> GetTestsPage(int pageNumber, int pageSize)
        {
            return await _storage.GetTestsByPageAsync(pageNumber, pageSize);
        }

        public virtual async Task<LinksPage> GetLinksPage(int testId)
        {
            return await _storage.GetLinksPageByTestIdAsync(testId);
            
        }
    }
}
