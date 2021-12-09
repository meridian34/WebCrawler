using System;
using System.Collections.Generic;

namespace WebCrawler.Services
{
    public class HtmlDocumentService 
    {
        const string StartOpeningLinkTag = "<a";
        const string EndTag = ">";
        const string Anchor = "#";
        const string PropSymbol = "?";
        const string HrefAttribute = "href";
        const string AttributeValueMarker = @"""";
        private string _htmlDocument;
        private List<string> _resultList = new List<string>();

        public virtual IEnumerable<string> GetLinks(string htmlBody)
        {
            _resultList.Clear();
            _htmlDocument = htmlBody;
            var startPosition = 0;
            RecursivelyFill(startPosition);

            return _resultList;
        }
        
        private void RecursivelyFill(int startPosition)
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

                    AddLink(link);
                }

                RecursivelyFill(endPositionTag++);
            }
        }
        private void AddLink(string link)
        {
            if (link.Contains(Anchor))
            {
                var endPositionLink = link.IndexOf(Anchor, 0);
                AddLink(link.Substring(0, endPositionLink));
            }
            else if (link.Contains(PropSymbol))
            {
                return;
            }
            else
            {
                _resultList.Add(link);
            }
        }
    }
}
