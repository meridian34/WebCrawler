using System.Linq;
using WebCrawler.Services.Models;
using WebCrawler.WebApplication.ViewModels;

namespace WebCrawler.WebApplication.Services
{
    public class Mapper
    {
        public TestsPageViewModel TestPageToTestPageViewModel(TestsPage testsPage)
        {
            return new TestsPageViewModel
            {
                CurrentPage = testsPage.CurrentPage,
                ItemsCount = testsPage.ItemsCount,
                TotalPages = testsPage.TotalPages,
                Tests = testsPage.Tests.Select(x => new TestViewModel
                {
                    Id = x.Id,
                    TestDateTime = x.TestDateTime,
                    UserLink = x.UserLink
                })
            };
        }

        public LinksPageViewModel LinksPageToLinksPageViewModel(LinksPage linksPage)
        {
            return new LinksPageViewModel
            {
                Links = linksPage.Links
                 .Select(x => new LinkViewModel
                 {
                     ElapsedMilliseconds = x.ElapsedMilliseconds,
                     FromHtml = x.FromHtml,
                     FromSitemap = x.FromSitemap,
                     Url = x.Url,
                     Id = x.Id
                 }),
                HtmlLinks = linksPage.HtmlLinks,
                SitemapLinks = linksPage.SitemapLinks,
                Url = linksPage.Url
            };
        }
    }
}
