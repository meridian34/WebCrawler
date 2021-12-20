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

namespace WebCrawler.Tests.Services
{
    public class WebCrawlerServiceTests
    {

        private readonly Mock<UrlValidatorService> _urlValidatorService;
        private readonly Mock<WebRequestService> _requestService;
        private readonly Mock<HtmlCrawler> _htmlCrawler;
        private readonly Mock<SitemapCrawler> _sitemapCrawler;


        public WebCrawlerServiceTests()
        {
            _urlValidatorService = new Mock<UrlValidatorService>();
            var linkConvertorService = new Mock<LinkConvertorService>();
            _requestService = new Mock<WebRequestService>(new Mock<HttpClientService>().Object);
            var htmlParser = new Mock<HtmlParser>(_urlValidatorService.Object, linkConvertorService.Object);
            _htmlCrawler = new Mock<HtmlCrawler>(_requestService.Object, htmlParser.Object, linkConvertorService.Object);
            var sitemapParser = new Mock<SitemapParser>(_urlValidatorService.Object);
            _sitemapCrawler = new Mock<SitemapCrawler>(_requestService.Object, sitemapParser.Object, linkConvertorService.Object);
        }

        [Fact]
        public async Task GetLinks_Uri_ShoudReturnResultListAsync()
        {
            // arrange
            _urlValidatorService.SetupSequence(x => x.ValidateUrl(It.IsAny<Uri>())).Returns(true);
            var inputUrl = new Uri("https://www.example.com/");
            var resultCrawler = new List<Link> { new Link { Url = inputUrl, FromHtml = true } };
            var resultSitemap = new List<Link> { new Link { Url = inputUrl, FromSitemap = true } };
            var expectedValue = new List<Link> { new Link { Url = inputUrl, FromHtml = true, FromSitemap = true } };
            _htmlCrawler.Setup(x => x.RunCrawlerAsync(It.IsAny<Uri>())).ReturnsAsync(resultCrawler);
            _sitemapCrawler.Setup(x => x.RunCrawlerAsync(It.IsAny<Uri>())).ReturnsAsync(resultSitemap);

            var webCrawler = new WebCrawlerService(_htmlCrawler.Object, _sitemapCrawler.Object, _urlValidatorService.Object, _requestService.Object);

            //act
            var result = await webCrawler.GetLinksAsync(inputUrl);

            //assert
            Assert.Collection(result, item => new Link { Url = inputUrl, FromHtml = true, FromSitemap = true });
        }

        [Fact]
        public async Task GetPerfomanceDataCollection_Uri_ShoudReturnPerfomanceListAsync()
        {
            // arrange
            var inputUrl = new Uri("https://www.example.com/");
            var inputList = new List<Link> { new Link() };
            _requestService.Setup(x => x.GetElapsedTimeForLinksAsync(It.IsAny<List<Link>>())).ReturnsAsync(new PerfomanceData[] { new PerfomanceData { ElapsedMilliseconds = 10, Url = inputUrl } });

            var webCrawler = new WebCrawlerService(_htmlCrawler.Object, _sitemapCrawler.Object, _urlValidatorService.Object, _requestService.Object);

            //act
            var result = await webCrawler.GetPerfomanceDataCollectionAsync(inputList);

            //assert
            Assert.Collection(result, item => new PerfomanceData { ElapsedMilliseconds = 10, Url = inputUrl });
        }
    }
}
