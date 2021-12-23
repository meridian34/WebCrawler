using Moq;
using System;
using System.Linq;
using WebCrawler.Services;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests.Services.Parsers
{
    public class HtmlParserTests
    {
        private readonly Mock<UrlValidatorService> _urlValidatorService;
        private readonly Mock<LinkConvertorService> _linkConvertorService;

        public HtmlParserTests()
        {
            _urlValidatorService = new Mock<UrlValidatorService>();
            _linkConvertorService = new Mock<LinkConvertorService>();
        }


        [Fact(Timeout = 1000)]
        public void GetHtmlLinks_HtmlDataWithRelativeLink_ShouldReturnLinksList()
        {
            // arrange
            var html = @" <a href=""/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>";
            var expectedValue = new Uri[] { new Uri("https://www.example.com/latest-projects/") };
            var root = new Uri("https://www.examplep.com/");
            var link =  new Uri("/latest-projects/", UriKind.Relative);
            var convertResult = new Uri("https://www.example.com/latest-projects/");
            _linkConvertorService.SetupSequence(x => x.GetRootUrl(It.IsAny<Uri>())).Returns(root);
            _urlValidatorService.SetupSequence(x => x.ValidateLink(It.Is<Uri>(s => s == link))).Returns(true);
            _urlValidatorService.SetupSequence(x => x.ValidateUrl(It.Is<Uri>(s => s == link))).Returns(false);
            _linkConvertorService.SetupSequence(x => x.ConvertRelativeToAbsolute(It.Is<Uri>(s => s == link), It.Is<Uri>(s => s == root))).Returns(convertResult);
            var service = new HtmlParser(_urlValidatorService.Object, _linkConvertorService.Object);

            //act
            var result = service.GetHtmlLinks(html, root).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact(Timeout = 1000)]
        public void GetHtmlLinks_HtmlDataWithAbsoluteLink_ShouldReturnLinksList()
        {
            // arrange
            var html = @" <a href=""https://www.example.com/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>";
            var expectedValue = new Uri[] { new Uri("https://www.example.com/latest-projects/") };
            var sourceUrl =  new Uri("https://www.example.com/123");
            var root = new Uri("https://www.example.com/");
            var link =  new Uri("https://www.example.com/latest-projects/");
            _linkConvertorService.SetupSequence(x => x.GetRootUrl(It.Is<Uri>(s => s == sourceUrl))).Returns(root);
            _urlValidatorService.SetupSequence(x => x.ValidateLink(It.Is<Uri>(s => s == link))).Returns(true);
            _urlValidatorService.SetupSequence(x => x.ValidateUrl(It.Is<Uri>(s => s == link))).Returns(true);
            var service = new HtmlParser(_urlValidatorService.Object, _linkConvertorService.Object);

            //act
            var result = service.GetHtmlLinks(html, sourceUrl).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }
    }
}
