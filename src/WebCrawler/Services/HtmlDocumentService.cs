using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        private const string _linkTag = "a";
        private const string _anchor = "#";
        private const string _hrefAttribute = "href";

        private const string Anchor = "#";
        private const string HrefAttribute = "href";
        private const string AttributeValueMarker = @"""";
        private string _htmlDocument;

        public IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            _htmlDocument = htmlBody;
            return FindNext(0);

        }
        
        private IReadOnlyCollection<string> FindNext(int startPosition)
        {
            var resultList = new List<string>();
            var startPositionTag = _htmlDocument.IndexOf("<a", startPosition);

            if (startPositionTag > startPosition)
            {
                var endPositionTag = _htmlDocument.IndexOf(">", startPositionTag);
                var sub = _htmlDocument.Substring(startPositionTag, endPositionTag - startPositionTag);
                var positionHref = sub.IndexOf(HrefAttribute, 0);
                if (positionHref > -1)
                {
                    var startLink = sub.IndexOf(AttributeValueMarker, positionHref) + 1;
                    var endLink = sub.IndexOf(AttributeValueMarker, startLink);
                    var sub2 = sub.Substring(startLink, endLink - startLink);
                    if (!sub2.Contains(Anchor))
                    {
                        resultList.Add(sub2);
                    }
                }
                resultList.AddRange(FindNext(endPositionTag++));

            }

            return resultList;
        }
    }
}
