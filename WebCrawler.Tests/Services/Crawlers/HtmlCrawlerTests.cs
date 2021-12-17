using Moq;
using System;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services;
using WebCrawler.Services.Crawlers;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests.Services.Crawlers
{
    public class HtmlCrawlerTests
    {
        [Fact]
        public async Task RunCrawler_Url_ShouldReturnLinkListAsync()
        {
            // arrange
            var firstUrl = new Uri("https://www.ukad-group.com/");
            var baseUrl = new Uri ("https://www.ukad-group.com/");
            var html = @" <a href=""/latest-projects/""</a>";
            var link = new Uri("/latest-projects/", UriKind.Relative);

            var urlValidator = new Mock<UrlValidatorService>();
            var requestService = new Mock<WebRequestService>(new Mock<HttpClientService>().Object);
            var linkConvertor = new Mock<LinkConvertorService>();
            var parserService = new Mock<HtmlParser>(urlValidator.Object, linkConvertor.Object);

            linkConvertor.SetupSequence(x => x.GetRootUrl(It.Is<Uri>(s => s == firstUrl))).Returns(baseUrl);
            requestService.SetupSequence(x => x.DownloadAsync(It.Is<Uri>(s => s == firstUrl))).ReturnsAsync(html);
            parserService.SetupSequence(x => x.GetHtmlLinks(It.IsAny<string>(), It.Is<Uri>(s => s == firstUrl))).Returns(new Uri[] { });

            var service = new HtmlCrawler(requestService.Object, parserService.Object, linkConvertor.Object);

            //act
            var result = await service.RunCrawlerAsync(firstUrl);

            //assert
            Assert.Collection(result, item => new Link { IsCrawler = true, Url = baseUrl });
        }
    }
}
