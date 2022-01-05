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
                Tests = testsPage.Tests
            };
        }

        public LinksPageViewModel LinksPageToLinksPageViewModel(LinksPage linksPage)
        {
            return new LinksPageViewModel
            {
                Links = linksPage.Links,
                Url = linksPage.Url
            };
        }
    }
}
