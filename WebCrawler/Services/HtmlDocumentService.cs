﻿using System.Collections.Generic;

namespace WebCrawler.Services
{
    public class HtmlDocumentService 
    {
        const string StartOpeningLinkTag = "<a";
        const string EndTag = ">";
        const string Anchor = "#";
        const string HrefAttribute = "href";
        const string AttributeValueMarker = @"""";
        private string _htmlDocument;
        private List<string> ResultList = new List<string>();

        public virtual IReadOnlyCollection<string> GetLinks(string htmlBody)
        {
            ResultList.Clear();
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
