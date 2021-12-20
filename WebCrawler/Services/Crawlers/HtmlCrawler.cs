using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class HtmlCrawler
    {
        private readonly WebRequestService _requestService;
        private readonly HtmlParser _parserService;
        private readonly LinkConvertorService _convertor;

        public HtmlCrawler(WebRequestService requestService, HtmlParser parserService, LinkConvertorService linkConvertorService)
        {
            _requestService = requestService;
            _parserService = parserService;
            _convertor = linkConvertorService;
        }

        public virtual async Task<IEnumerable<Link>> RunCrawlerAsync(Uri url)
        {
            var baseUrl = _convertor.GetRootUrl(url);
            var resultList = new List<CrawlerData> { new CrawlerData{ Url = baseUrl } };
            
            while (resultList.Any(x=> !x.Scanned ))
            {
                var link = resultList.First(x => !x.Scanned);
                var html = await _requestService.DownloadAsync(link.Url);
                link.Scanned = true;
                var parsedLinks = _parserService.GetHtmlLinks(html, link.Url);
                
                resultList.AddRange(GetUniqueLinks(parsedLinks, resultList));
            }

            return resultList.Select(x=> new Link { Url = x.Url, FromHtml = true });
        }

        private IEnumerable<CrawlerData> GetUniqueLinks(IEnumerable<Uri> filterLinks, IEnumerable<CrawlerData> targetList)
        {
            var resultList = new Queue<CrawlerData>();
            foreach (var link in filterLinks)
            {
                var resultsContainsUrl = targetList.Any(x => x.Url == link);
                if (!resultsContainsUrl)
                {
                    resultList.Enqueue(new CrawlerData { Url = link });
                }
            }
            return resultList;
        }
    }
}
