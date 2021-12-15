using Moq;
using System.Linq;
using WebCrawler.Services;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests
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

        
        [Fact]
        public void GetHtmlLinks_HtmlDataWithRelativeLink_ShouldReturnLinksList()
        {
            // arrange
            var html = @" <a href=""/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>";
            var expectedValue = new string[] { "https://www.ukad-group.com/latest-projects/" };
            var root = "https://www.ukad-group.com/";
            var link = "/latest-projects/";
            var convertResult = "https://www.ukad-group.com/latest-projects/";

            _urlValidatorService.SetupSequence(x => x.LinkIsValid(It.Is<string>(s => s == link))).Returns(true);
            _urlValidatorService.SetupSequence(x => x.UrlIsValid(It.Is<string>(s => s == link))).Returns(false);
            _urlValidatorService.SetupSequence(x => x.ContainsBaseUrl(It.Is<string>(s => s == link), It.Is<string>(s => s == root))).Returns(false);
            _linkConvertorService.SetupSequence(x => x.ConvertRelativeToAbsolute(It.Is<string>(s => s == link), It.Is<string>(s => s == root))).Returns(convertResult);
            var service = new HtmlParser(_urlValidatorService.Object, _linkConvertorService.Object);

            //act
            var result = service.GetHtmlLinks(html, root).ToArray();
            
            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void GetHtmlLinks_HtmlDataWithAbsoluteLink_ShouldReturnLinksList()
        {
            // arrange
            var html = @" <a href=""https://www.ukad-group.com/latest-projects/"" class=""navigation - new__link"" title=""PORTFOLIO""></a>";
            var expectedValue = new string[] { "https://www.ukad-group.com/latest-projects/" };
            var sourceUrl = "https://www.ukad-group.com/123";
            var root = "https://www.ukad-group.com/";
            var link = "https://www.ukad-group.com/latest-projects/";
            _linkConvertorService.SetupSequence(x => x.GetRootUrl(It.Is<string>(s => s == sourceUrl))).Returns(root);
            _urlValidatorService.SetupSequence(x => x.LinkIsValid(It.Is<string>(s => s == link))).Returns(true);
            _urlValidatorService.SetupSequence(x => x.UrlIsValid(It.Is<string>(s => s == link))).Returns(true);
            _urlValidatorService.SetupSequence(x => x.ContainsBaseUrl(It.Is<string>(s => s == link), It.Is<string>(s => s == root))).Returns(true);
            var service = new HtmlParser(_urlValidatorService.Object, _linkConvertorService.Object);

            //act
            var result = service.GetHtmlLinks(html, sourceUrl).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }
    }
}
