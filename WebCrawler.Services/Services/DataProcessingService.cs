using System;
using System.Threading.Tasks;
using WebCrawler.Services.Exceptions;
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

        public virtual async Task StartCrawlingSiteAsync(string inputUrl)
        {
            Uri url;
            var urlCreated = Uri.TryCreate(inputUrl, UriKind.Absolute, out url);
            if (!urlCreated)
            {
                throw new ValidationException("Invalid URL");
            }


            var links = await _webCrawler.GetLinksAsync(url);
            var perfomanceData = await _webCrawler.GetPerfomanceDataCollectionAsync(links);
            await _storage.SaveAsync(url.OriginalString, links, perfomanceData);
        } 

        public virtual async Task<TestsPage> GetTestsPageAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ValidationException("Params 'pageNumber' must be > 0 and 'pageSize' must be > 0 ");
            }

            return await _storage.GetTestsByPageAsync(pageNumber, pageSize);
        }

        public virtual async Task<LinksPage> GetLinksPageAsync(int testId)
        {
            if (testId <= 0)
            {
                throw new ValidationException("Input parameter 'testId' must be >= 0");
            }

            return await _storage.GetLinksPageByTestIdAsync(testId);
        }
    }
}
