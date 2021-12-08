using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class SiteScanService 
    {
        private readonly WebHandlerService _webHandlerService;
        private readonly HtmlDocumentService _htmlDocumentService;
        private List<HttpScanResult> _scanList;
        private string _root;

        public SiteScanService(
            WebHandlerFactory webHandlerFactory,
            HtmlDocumentService htmlDocumentService
            )
        {
            _scanList = new List<HttpScanResult>();
            _webHandlerService = webHandlerFactory.CreateForSiteScan();
            _htmlDocumentService = htmlDocumentService;
            
        }

        public async Task<IReadOnlyCollection<HttpScanResult>> ScanSiteAsync(string url)
        {
            _scanList.Clear();
            Uri uriResult = new Uri(url);
            _root = uriResult.GetLeftPart(UriPartial.Authority);
            _scanList.Add(new HttpScanResult() { Url = _root, IsCrawled = false });

            while (_scanList.Any(x => !x.IsCrawled))
            {
                var scanResults = await _webHandlerService.ScanUrlConcurencyAsync(_scanList.Where(x => x.IsCrawled == false).ToList());
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
            Uri uriResult;
            var isAbsoluteLink = Uri.TryCreate(link, UriKind.Absolute, out uriResult);

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
                var root = new Uri(_root);
                var newUri = new Uri(root, link);
                AddInScanList(newUri.ToString());
            }
        }
        private void AddInScanList(IReadOnlyCollection<string> links)
        {
            foreach(var link in links)
            {
                AddInScanList(link);
            }
        }
    }
}
