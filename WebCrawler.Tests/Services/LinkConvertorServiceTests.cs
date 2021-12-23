using System;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests.Services
{
    public class LinkConvertorServiceTests
    {
        private readonly LinkConvertorService _service = new LinkConvertorService();

        [Theory(Timeout = 1000)]
        [InlineData("https://www.example.com/somegroup/", "/123/", "https://www.example.com/123/")]
        [InlineData("https://www.example.com/", "/somegroup/", "https://www.example.com/ukadgroup/")]
        public void ConvertRelativeToAbsolute_LinkAndBasePath_ShouldReturnAbsolutePath(string rootUrl, string link, string expectedUrl)
        {
            // arrange
            var baseUrl = new Uri(rootUrl);
            var url = new Uri(link, UriKind.Relative);
            var expectedValue = new Uri(expectedUrl);

            //act
            var result = _service.ConvertRelativeToAbsolute(url, baseUrl);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory(Timeout = 1000)]
        [InlineData("https://www.example.com/", "https://www.example.com/sitemap.xml")]
        [InlineData("https://www.example.com/somegroup/", "https://www.example.com/sitemap.xml")]
        [InlineData("https://www.example.com/123/", "https://www.example.com/sitemap.xml")]
        public void GetDefaultSitemap_Url_ShouldReturnDefaultSitemapPath(string link, string expectedUrl)
        {
            // arrange
            var url = new Uri(link);
            var expectedValue = new Uri(expectedUrl);

            //act
            var result = _service.GetDefaultSitemap(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact(Timeout = 1000)]
        public void GetDefaultSitemap_EmptyString_ShouldThrowNullRefferenceException()
        {
            // arrange
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => _service.GetDefaultSitemap(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Theory(Timeout = 1000)]
        [InlineData("https://www.example.com/", "https://www.example.com/")]
        [InlineData("https://www.example.com/somegroup/", "https://www.example.com/")]
        [InlineData("https://www.example.com/123/", "https://www.example.com/")]
        [InlineData("https://www.example.com/somegroup/?param1=0", "https://www.example.com/")]
        public void GetRootUrl_Url_ShouldReturnRootUrl(string link, string expectedUrl)
        {
            // arrange
            var url = new Uri(link);
            var expectedValue = new Uri(expectedUrl);

            //act
            var result = _service.GetRootUrl(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact(Timeout = 1000)]
        public void GetRootUrl_EmptyString_ShouldThrowNullRefferenceException()
        {
            // arrange
            var service = new LinkConvertorService();
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<System.NullReferenceException>(() => service.GetRootUrl(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }
    }
}
