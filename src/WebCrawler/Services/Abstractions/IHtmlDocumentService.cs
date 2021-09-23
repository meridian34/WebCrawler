using System.Collections.Generic;

namespace WebCrawler.Services.Abstractions
{
    public interface IHtmlDocumentService
    {
        public IReadOnlyCollection<string> GetLinks(string htmlBody);
    }
}
