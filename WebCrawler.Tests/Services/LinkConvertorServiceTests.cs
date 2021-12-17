using System;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests.Services
{
    public class LinkConvertorServiceTests
    {
        private readonly LinkConvertorService _service = new LinkConvertorService();

        [Theory]
        [InlineData("https://www.ukad-group.com/ukadgroup/", "/123/", "https://www.ukad-group.com/123/")]
        [InlineData("https://www.ukad-group.com/", "/ukadgroup/", "https://www.ukad-group.com/ukadgroup/")]
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

        [Theory]
        [InlineData("https://www.ukad-group.com/", "https://www.ukad-group.com/sitemap.xml")]
        [InlineData("https://www.facebook.com/ukadgroup/", "https://www.facebook.com/sitemap.xml")]
        [InlineData("https://www.ukad-group.com/123/", "https://www.ukad-group.com/sitemap.xml")]
        [InlineData("https://www.ukad-group.com/ukadgroup/", "https://www.ukad-group.com/sitemap.xml")]
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

        [Fact]
        public void GetDefaultSitemap_EmptyString_ShouldThrowNullRefferenceException()
        {
            // arrange
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => _service.GetDefaultSitemap(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Theory]
        [InlineData("https://www.ukad-group.com/", "https://www.ukad-group.com/")]
        [InlineData("https://www.facebook.com/ukadgroup/", "https://www.facebook.com/")]
        [InlineData("https://www.ukad-group.com/123/", "https://www.ukad-group.com/")]
        [InlineData("https://www.ukad-group.com/ukadgroup/?param1=0", "https://www.ukad-group.com/")]
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

        [Fact]
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
