using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Services.Tests.Crawlers
{
    public class SitemapCrawlerTests
    {
        [Fact]
        public async Task RunCrawler_Url_ShouldReturnLinkListAsync()
        {
            // arrange
            var firstUrl = new Uri("https://www.ukad-group.com/");
            var sitemapUrl = new Uri("https://www.ukad-group.com/sitemap.xml");
            var xml = @"<loc>https://www.ukad-group.com/latest-projects/</loc> <loc>https://www.ukad-group.com/</loc>";
            var firstValue = new Uri("https://www.ukad-group.com/latest-projects/");
            var secondValue = new Uri("https://www.ukad-group.com/");
            var link = new Uri("/latest-projects/", UriKind.Relative);

            var urlValidator = new Mock<UrlValidatorService>();
            var requestService = new Mock<WebRequestService>(new Mock<HttpClientService>().Object);
            var linkConvertor = new Mock<LinkConvertorService>();
            var parserService = new Mock<SitemapParser>(urlValidator.Object);

            linkConvertor.SetupSequence(x => x.GetDefaultSitemap(It.Is<Uri>(s => s == firstUrl))).Returns(sitemapUrl);
            requestService.SetupSequence(x => x.DownloadAsync(It.Is<Uri>(s => s == sitemapUrl)))
                .ReturnsAsync(xml)
                .ReturnsAsync("")
                .ReturnsAsync("");
            parserService.SetupSequence(x => x.GetSitemapLinks(It.IsAny<string>()))
                .Returns(new List<Uri> { firstValue, secondValue })
                .Returns(new List<Uri>())
                .Returns(new List<Uri>());

            var service = new SitemapCrawler(requestService.Object, parserService.Object, linkConvertor.Object);

            //act
            var result = await service.RunCrawlerAsync(firstUrl);

            //assert
            Assert.Collection(result, firstItem => new Uri("https://www.ukad-group.com/latest-projects/"), secondItem => new Uri("https://www.ukad-group.com/"));
        }
    }
}
