using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class SiteScanService : ISiteScanService
    {
        private readonly IWebHandlerService _webHandlerService;
        private readonly IHtmlDocumentService _htmlDocumentService;
        private readonly IUrlsRepositoryService _repositoryService;
        private List<HttpScanResult> _results;

        public SiteScanService(
            IWebHandlerFactory webHandlerFactory,
            IHtmlDocumentService htmlDocumentService,
            IUrlsRepositoryService linkRepositoryService)
        {
            _results = new List<HttpScanResult>();
            _webHandlerService = webHandlerFactory.CreateForSiteScan();
            _htmlDocumentService = htmlDocumentService;
            _repositoryService = linkRepositoryService;
        }

        public async Task<IReadOnlyCollection<HttpScanResult>> ScanAsync(string url)
        {
            _results.Clear();
            await _repositoryService.InitRootAsync(url);
            while (_repositoryService.Count > 0)
            {
                var urls = await _repositoryService.GetListUniqueUrlsAsync();
                var results = await _webHandlerService.ScanUrlConcurencyAsync(urls);
                var contents = results.Where(x => x.Content != null && x.Exception == null)
                                        .Select(x => x.Content);
                foreach (var body in contents)
                {
                    var links = _htmlDocumentService.GetLinks(body);
                    await _repositoryService.AddLinkAsync(links);
                }

                _results.AddRange(results);
            }

            return _results;
        }
    }
}
