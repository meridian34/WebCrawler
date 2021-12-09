using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class SiteScanService 
    {
        private readonly WebHandlerService _webHandlerService;
        private readonly HtmlDocumentService _htmlDocumentService;
        private List<HttpScanResult> _scanList;
        private string _root;

        public SiteScanService()
        {
            _scanList = new List<HttpScanResult>();
            _webHandlerService = new WebHandlerService(WebHandlerType.SiteScan);
            _htmlDocumentService = new HtmlDocumentService();
        }

        public virtual async Task<IEnumerable<HttpScanResult>> ScanSiteAsync(string url)
        {
            _scanList.Clear();
            Uri uriResult = new Uri(url);
            _root = new Uri(uriResult.GetLeftPart(UriPartial.Authority)).ToString();
            _scanList.Add(new HttpScanResult() { Url = _root, IsCrawled = false });

            while (_scanList.Any(x => !x.IsCrawled))
            {
                var scanResults = await _webHandlerService.ScanUrlConcurencyAsync(_scanList.Where(x => x.IsCrawled == false));
                var contentList = scanResults.Where(x => x.Content != null && x.Exception == null)
                                        .Select(x => x.Content);

                foreach (var htmlContent in contentList)
                {
                    var links = _htmlDocumentService.GetLinks(htmlContent);
                    AddInScanList(links);
                }
            }

            return _scanList;
        }

        private void AddInScanList(string link)
        {   
            
            var isAbsoluteLink = Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult);
            if (isAbsoluteLink)
            {
                var isUniqueLink = !_scanList.Any(x => x.Url == link);
                var isValidUrl = (uriResult.Scheme == Uri.UriSchemeHttp ||
                        uriResult.Scheme == Uri.UriSchemeHttps) &&
                        isUniqueLink && link.Contains(_root);

                if (isValidUrl)
                {
                    _scanList.Add(new HttpScanResult() { Url = link, IsCrawled = false });
                }
            }
            else
            {
                var isRelativeLinkSource = link.Contains(":");
                if (isRelativeLinkSource)
                {
                    return;
                }

                var root = new Uri(_root);
                var newUri = new Uri(root, link);
                AddInScanList(newUri.ToString());
            }
        }
        private void AddInScanList(IEnumerable<string> links)
        {
            foreach(var link in links)
            {
                AddInScanList(link);
            }
        }
    }
}
