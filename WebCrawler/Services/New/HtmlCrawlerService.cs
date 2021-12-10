using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                    var parsedLinks = _parserService.GetLinks(html);

                    foreach(var newLink in parsedLinks)
                    {
                        linkQueue.Enqueue(newLink);
                    }
                }
                else
                {
                    var conRes = _linkConverterService.ConvertToUrl(link, baseUrl);
                }
            }

            return resultList;
        }


        private string GetRootUrl(string url)
        {
            var basePath = new Uri(url).GetLeftPart(UriPartial.Authority);
            return basePath;
        }
    }
}
