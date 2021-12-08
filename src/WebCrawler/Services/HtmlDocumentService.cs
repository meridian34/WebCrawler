using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        private const string StartOpeningLinkTag = "<a";
        private const string EndTag = ">";
        private const string Anchor = "#";
        private const string HrefAttribute = "href";
        private const string AttributeValueMarker = @"""";
        private string _htmlDocument;
        private List<string> ResultList = new List<string>();

        public IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            _htmlDocument = htmlBody;
            FindNext(0);
            return ResultList;

        }
        
        private void FindNext(int startPosition)
        {   
            var startPositionTag = _htmlDocument.IndexOf(StartOpeningLinkTag, startPosition);

            if (startPositionTag > startPosition)
            {
                var endPositionTag = _htmlDocument.IndexOf(EndTag, startPositionTag);
                var tagBody = _htmlDocument.Substring(startPositionTag, endPositionTag - startPositionTag);
                var positionHref = tagBody.IndexOf(HrefAttribute, 0);
                if (positionHref > -1)
                {
                    var startLink = tagBody.IndexOf(AttributeValueMarker, positionHref) + 1;
                    var endLink = tagBody.IndexOf(AttributeValueMarker, startLink);
                    var link = tagBody.Substring(startLink, endLink - startLink);
                    if (!link.Contains(Anchor))
                    {
                        ResultList.Add(link);
                    }
                }
                FindNext(endPositionTag++);

            }
        }
    }
}
