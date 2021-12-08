using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Services
{
    public class HtmlLinkParserService
    {
        private const string Anchor = "#";
        private const string HrefAttribute = "href";
        private const string AttributeValueMarker = @"""";
        private string _htmlDocument;
        public IReadOnlyCollection<string> GetLink(string htmlDocument)
        {
            _htmlDocument = htmlDocument;
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
                    var endLink = sub.IndexOf(AttributeValueMarker, startLink) - 1;
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
