namespace WebCrawler.WebApplication.ViewModels
{
    public class LinkViewModel
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool FromSitemap { get; set; }

        public bool FromHtml { get; set; }

        public int? ElapsedMilliseconds { get; set; }

    }
}
