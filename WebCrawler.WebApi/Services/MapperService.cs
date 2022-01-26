using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Services.Models;
using WebCrawler.WebApi.DTOs;

namespace WebCrawler.WebApi.Services
{
    public class MapperService
    {
        public TestDetailsPageDto MapModelToDetailsDto(LinksPage linksPage)
        {
            var result = new TestDetailsPageDto
            {
                Url = linksPage.Url,
                Links = linksPage.Links.Select(x => new LinkDto
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

        public TestsPageDto MapModelToTestsDto(TestsPage testsPage)
        {
            var result = new TestsPageDto
            {
                CurrentPage = testsPage.CurrentPage,
                ItemsCount = testsPage.ItemsCount,
                TotalPages = testsPage.TotalPages,
                Tests = testsPage.Tests.Select(x => new TestDto
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
