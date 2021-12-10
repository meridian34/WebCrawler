using System;
using System.Collections.Generic;
using System.Linq;
using WebCrawler.Models;

namespace WebCrawler.Services.New
{
    public class HtmlCrawlerService
    {
        private readonly UrlValidatorService _urlValidator;
        private readonly RequestService _requestService;
        private readonly ParserService _parserService;
        private readonly LinkConverterService _linkConverterService;

        public HtmlCrawlerService(
            UrlValidatorService urlValidatorService,
            RequestService requestService,
            ParserService parserService,
            LinkConverterService linkConverterService)
        {
            _urlValidator = urlValidatorService;
            _requestService = requestService;
            _parserService = parserService;
            _linkConverterService = linkConverterService;
        }

        public virtual IEnumerable<CrawlResult> RunCrawler(string url)
        {   
            var baseUrl = GetRootUrl(url);
            var linkQueue = new Queue<string>();
            var resultList = new List<CrawlResult>();
            linkQueue.Enqueue(baseUrl);

            while (linkQueue.Count > 0)
            {
                var link = linkQueue.Dequeue();
                var resultContainsLink = resultList.Any(x => x.Url == link);

                if (!_urlValidator.IsValidLink(link) || resultContainsLink)
                {
                    continue;
                }

                var linkIsCorrectUrl = _urlValidator.IsCorrectLink(link)
                    && _urlValidator.ContainsBaseUrl(link, baseUrl);

                if (linkIsCorrectUrl)
                {
                    var result = _requestService.ScanUrlAsync(link, out string html);
                    result.IsSiteScan = true;
                    resultList.Add(result);

                    var parsedLinks = _parserService.GetHtmlLinks(html);
                    foreach(var newLink in parsedLinks)
                    {
                       var correctLink = _urlValidator.ContainsBaseUrl(newLink, baseUrl) 
                            ? newLink 
                            : _linkConverterService.ConvertRelativeToAbsolute(newLink, baseUrl);
                        
                        AnalyzeLink(correctLink, linkQueue);
                    }
                }
            }
            
            return resultList;
        }

        private void AnalyzeLink(string parsedLinks, Queue<string> queue)
        {
            if(!queue.Any(x=>x == parsedLinks))
            {
                queue.Enqueue(parsedLinks);
            }
        }

        private string GetRootUrl(string url)
        {
            var basePath = $"{new Uri(url).GetLeftPart(UriPartial.Authority)}/";
            return basePath;
        }
    }
}
