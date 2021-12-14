using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebCrawler.Models;
using WebCrawler.Services.Parsers;

namespace WebCrawler.Services.Crawlers
{
    public class HtmlCrawler
    {
        private readonly UrlValidatorService _validator;
        private readonly RequestService _requestService;
        private readonly HtmlParser _parserService;
        private readonly LinkConvertorService _convertor;

        public HtmlCrawler(
            UrlValidatorService urlValidatorService,
            RequestService requestService,
            HtmlParser parserService,
            LinkConvertorService linkConvertorService)
        {
            _validator = urlValidatorService;
            _requestService = requestService;
            _parserService = parserService;
            _convertor = linkConvertorService;
        }

        public virtual IEnumerable<Link> RunCrawler(string url)
        {
            var delay = 50;
            var baseUrl = _convertor.GetRootUrl(url);
            var linkQueue = new Queue<string>();
            var resultList = new List<Link>();
            linkQueue.Enqueue(baseUrl);

            while (linkQueue.Count > 0)
            {
                var link = linkQueue.Dequeue();
                var resultsContainsLink = resultList.Any(x => x.Url == link);
                var notValidLink = !_validator.LinkIsValid(link);
                var linkIsNotCorrect = !_validator.LinkIsCorrect(link)
                    || !_validator.ContainsBaseUrl(link, baseUrl);

                if (notValidLink || resultsContainsLink || linkIsNotCorrect)
                {
                    continue;
                }

                Thread.Sleep(delay);

                resultList.Add(new Link() { Url = link, IsCrawler = true });
                var html = _requestService.Download(link);
                var parsedLinks = _parserService.GetHtmlLinks(html);

                foreach (var newLink in parsedLinks)
                {


                    var correctLink = _validator.ContainsBaseUrl(newLink, baseUrl)
                         ? newLink
                         : _convertor.ConvertRelativeToAbsolute(newLink, baseUrl);

                    var notContainsLink = !linkQueue.Any(x => x == correctLink);

                    if (notContainsLink)
                    {
                        
                        linkQueue.Enqueue(correctLink);
                    }
                }
            }
            
            return resultList;
        }
    }
}
