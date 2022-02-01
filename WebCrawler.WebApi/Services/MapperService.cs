using System.Linq;
using WebCrawler.Services.Models;
using WebCrawler.WebApi.Models;

namespace WebCrawler.WebApi.Services
{
    public class MapperService
    {
        public TestDetailsPage MapModelToDetailsDto(LinksPage linksPage)
        {
            var result = new TestDetailsPage
            {
                Url = linksPage.Url,
                Links = linksPage.Links.Select(x => new WebApi.Models.Link
                {
                    ElapsedMilliseconds = x.ElapsedMilliseconds,
                    FromHtml = x.FromHtml,
                    FromSitemap = x.FromSitemap,
                    Id = x.Id,
                    Url = x.Url
                })
            };

            return result;
        }

        public WebApi.Models.TestsPage MapModelToTestsDto(WebCrawler.Services.Models.TestsPage testsPage)
        {
            var result = new WebApi.Models.TestsPage
            {
                CurrentPage = testsPage.CurrentPage,
                ItemsCount = testsPage.ItemsCount,
                TotalPages = testsPage.TotalPages,
                Tests = testsPage.Tests.Select(x => new TestResponse
                {
                    Id = x.Id,
                    TestDateTime = x.TestDateTime,
                    UserLink = x.UserLink
                })
            };

            return result;
        }
    }
}
