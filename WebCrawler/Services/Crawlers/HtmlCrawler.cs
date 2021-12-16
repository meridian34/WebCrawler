using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class HtmlCrawler
    {
        private readonly RequestService _requestService;
        private readonly HtmlParser _parserService;
        private readonly LinkConvertorService _convertor;

        public HtmlCrawler(
            RequestService requestService,
            HtmlParser parserService,
            LinkConvertorService linkConvertorService)
        {
            _requestService = requestService;
            _parserService = parserService;
            _convertor = linkConvertorService;
        }

        public virtual IEnumerable<Link> RunCrawler(Uri url)
        {
            var baseUrl = _convertor.GetRootUrl(url);
            var linkQueue = new Queue<Uri>();
            var resultList = new List<Link>();
            resultList.Add(new Link { IsCrawler = true, Url = baseUrl });
            linkQueue.Enqueue(baseUrl);

            while (linkQueue.Any())
            {
                var link = linkQueue.Dequeue();
                var html = _requestService.Download(link);
                var parsedLinks = _parserService.GetHtmlLinks(html, link);
                
                foreach (var newLink in parsedLinks)
                {
                    var queueContainsUrl = linkQueue.Any(x => x == newLink);
                    var resultsContainsUrl = resultList.Any(x => x.Url == newLink);

                    if (!resultsContainsUrl)
                    {
                        resultList.Add(new Link { IsCrawler = true, Url = newLink });
                    }

                    if (!resultsContainsUrl && !queueContainsUrl)
                    {
                        linkQueue.Enqueue(newLink);
                    }
                }
            }
            
            return resultList;
        }
    }
}
