﻿using System;
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

        public virtual IEnumerable<Uri> GetHtmlLinks(string html, Uri sourceUrl)
        {
            var linkList = new Queue<Uri>();

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
                    var parsedLink = tagBody.Substring(startLink, endLink - startLink);

                    var filtredLink = FilterLink(parsedLink, baseUrl);
                    if (filtredLink != null)
                    {
                        linkList.Enqueue(filtredLink);
                    }
                }

                position = endPositionTag + endTag.Length;
            }

            return linkList;
        }

        private Uri FilterLink(string parsedLink, Uri baseUrl)
        {
            var isCreated = Uri.TryCreate(parsedLink, UriKind.RelativeOrAbsolute, out Uri link);
            if (!isCreated)
            {
                return null;
            }

            var linkIsValid = _urlValidatorService.ValidateLink(link);
            var linkContainsBaseUrl = link.OriginalString.Contains(baseUrl.OriginalString); 

            if (linkIsValid && linkContainsBaseUrl)
            {
                return link;
            }
            else if (linkIsValid && !link.IsAbsoluteUri)
            {
                var absoluteUrl = _linkConvertorService.ConvertRelativeToAbsolute(link, baseUrl);
                return absoluteUrl;
            }

            return null;
        }
    }
}
