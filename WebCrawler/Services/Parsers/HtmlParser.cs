using System;
using System.Collections.Generic;

namespace WebCrawler.Services.Parsers
{
    public class HtmlParser
    {
        private readonly UrlValidatorService _urlValidatorService;
        private readonly LinkConvertorService _linkConvertorService;

        public HtmlParser(UrlValidatorService urlValidatorService, LinkConvertorService linkConvertorService)
        {
            _urlValidatorService = urlValidatorService;
            _linkConvertorService = linkConvertorService;
        }

        public virtual IEnumerable<string> GetHtmlLinks(string html, string sourceUrl)
        {
            var linkList = new List<string>();

            if (string.IsNullOrEmpty(html) )
            {
                return linkList;
            }
            var baseUrl = _linkConvertorService.GetRootUrl(sourceUrl);
            var startATag = "<a";
            var endTag = ">";
            var href = @"href=""";
            var endHref = @"""";
            var position = 0;

            while (position < html.Length)
            {
                var startPositionTag = html.IndexOf(startATag, position);

                if (startPositionTag == -1)
                {
                    break;
                }

                var endPositionTag = html.IndexOf(endTag, startPositionTag);
                var tagBody = html.Substring(startPositionTag, endPositionTag - startPositionTag);
                var positionHref = tagBody.IndexOf(href, 0);

                if (positionHref > -1)
                {
                    var startLink = positionHref + href.Length;
                    var endLink = tagBody.IndexOf(endHref, startLink);
                    var link = tagBody.Substring(startLink, endLink - startLink);

                    var linkIsValid = _urlValidatorService.LinkIsValid(link);
                    var linkIsUrl = _urlValidatorService.UrlIsValid(link);
                    var linkContainsBaseUrl = _urlValidatorService.ContainsBaseUrl(link, baseUrl);

                    if (linkIsValid && linkContainsBaseUrl)
                    {
                        linkList.Add(link);
                    }
                    else if (!linkIsUrl && linkIsValid && !linkContainsBaseUrl)
                    {
                        var absoluteUrl = _linkConvertorService.ConvertRelativeToAbsolute(link, sourceUrl);
                        linkList.Add(absoluteUrl);
                    }
                }

                position = endPositionTag + endTag.Length;
            }

            return linkList;
        }
    }
}
