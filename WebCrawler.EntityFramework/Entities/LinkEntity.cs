namespace WebCrawler.EntityFramework.Entities
{
    public class LinkEntity
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool FromSitemap { get; set; }

        public bool FromHtml { get; set; }

        public int? ElapsedMilliseconds { get; set; }

        public int TestId { get; set; }
        public TestEntity Test { get; set; }
    }
}
