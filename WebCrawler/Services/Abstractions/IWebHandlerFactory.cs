namespace WebCrawler.Services.Abstractions
{
    public interface IWebHandlerFactory
    {
        public WebHandlerService CreateForSiteScan();

        public WebHandlerService CreateForSiteMap();
    }
}
