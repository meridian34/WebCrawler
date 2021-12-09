namespace WebCrawler.Models
{
    public class CrawlResult
    {
        public string Url { get; set; }
        public int ElapsedMilliseconds { get; set; }
        public bool IsSiteScan { get; set; }
        public bool IsSiteMap { get; set; }
    }
}
