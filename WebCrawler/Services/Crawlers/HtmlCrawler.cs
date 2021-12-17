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

        public HtmlCrawler(
            WebRequestService requestService,
            HtmlParser parserService,
            LinkConvertorService linkConvertorService)
        {
            _requestService = requestService;
            _parserService = parserService;
            _convertor = linkConvertorService;
        }

        public virtual async Task<IEnumerable<Link>> RunCrawlerAsync(Uri url)
        {
            var baseUrl = _convertor.GetRootUrl(url);
            var resultList = new List<Link>();
            resultList.Add(new Link { Url = baseUrl });            

            while (resultList.Any(x=> !x.IsCrawler ))
            {
                var link = resultList.First(x => !x.IsCrawler);
                var html = await _requestService.DownloadAsync(link.Url);
                link.IsCrawler = true;
                var parsedLinks = _parserService.GetHtmlLinks(html, link.Url);

                FilterLinksAndAdd(parsedLinks, resultList);
            }

            return resultList;
        }

        private void FilterLinksAndAdd(IEnumerable<Uri> filterLinks, List<Link> resultList)
        {
            foreach (var link in filterLinks)
            {
                var resultsContainsUrl = resultList.Any(x => x.Url == link);
                if (!resultsContainsUrl)
                {
                    resultList.Add(new Link { Url = link });
                }
            }
        }
    }
}
