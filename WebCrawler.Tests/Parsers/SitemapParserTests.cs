using Moq;
using System.Linq;
using WebCrawler.Services;
using WebCrawler.Services.Parsers;
using Xunit;

namespace WebCrawler.Tests
{
    public class SitemapParserTests
    {
        private readonly Mock<UrlValidatorService> _urlValidatorService = new Mock<UrlValidatorService>();

        [Theory]
        [InlineData(@"<loc>https://www.ukad-group.com/</loc>", "https://www.ukad-group.com/")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/</loc> <loc>https://www.ukad-group.com/</loc>", "https://www.ukad-group.com/latest-projects/", "https://www.ukad-group.com/")]
        public void GetSitemapLinks_ValidSitemapXmlData_ShouldReturnLinksList(string html, params string[] expectedValue)
        {
            // arrange
            _urlValidatorService.Setup(x => x.UrlIsValid(It.IsAny<string>())).Returns(true);

            var service = new SitemapParser(_urlValidatorService.Object);

            //act
            var result = service.GetSitemapLinks(html).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/?param1=24</loc>")]
        [InlineData(@"<loc>https://www.ukad-group.com/latest-projects/#point1</loc>")]
        [InlineData(@"<loc>mailto:hi@ukad-group.com</loc>")]
        public void GetSitemapLinks_NotValidSitemapXmlData_ShouldReturnEmptyLinksList(string html, params string[] expectedValue)
        {
            // arrange
            _urlValidatorService.SetupSequence(x => x.UrlIsValid(It.IsAny<string>())).Returns(false);

            var service = new SitemapParser(_urlValidatorService.Object);

            //act
            var result = service.GetSitemapLinks(html).ToArray();

            //assert
            Assert.Equal(result, expectedValue);
        }
    }
}
