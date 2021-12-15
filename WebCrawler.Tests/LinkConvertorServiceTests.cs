using System;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests
{
    public class LinkConvertorServiceTests
    {
        [Theory]
        [InlineData("https://www.ukad-group.com/", null, "https://www.ukad-group.com/")]
        [InlineData("https://www.ukad-group.com/", "", "https://www.ukad-group.com/")]
        [InlineData("https://www.ukad-group.com/ukadgroup/", "/123/", "https://www.ukad-group.com/123/")]
        [InlineData("https://www.ukad-group.com/", "/ukadgroup/", "https://www.ukad-group.com/ukadgroup/")]
        public void ConvertRelativeToAbsolute_LinkAndBasePath_ShouldReturnAbsolutePath(string baseUrl, string url, string expectedValue)
        {
            // arrange
            var service = new LinkConvertorService();

            //act
            var result = service.ConvertRelativeToAbsolute(url, baseUrl);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("https://www.ukad-group.com/", "https://www.ukad-group.com/sitemap.xml")]
        [InlineData("https://www.facebook.com/ukadgroup/", "https://www.facebook.com/sitemap.xml")]
        [InlineData("https://www.ukad-group.com/123/", "https://www.ukad-group.com/sitemap.xml")]
        [InlineData("https://www.ukad-group.com/ukadgroup/", "https://www.ukad-group.com/sitemap.xml")]
        public void GetDefaultSitemap_Url_ShouldReturnDefaultSitemapPath(string url, string expectedValue)
        {
            // arrange
            var service = new LinkConvertorService();

            //act
            var result = service.GetDefaultSitemap(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void GetDefaultSitemap_Null_ShouldThrowNullRefferenceException()
        {
            // arrange
            var service = new LinkConvertorService();            
            var expectedValue = "Invalid URI: The URI is empty.";

            //act
            var result = Assert.Throws<UriFormatException>(() => service.GetDefaultSitemap(string.Empty));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Fact]
        public void GetDefaultSitemap_EmptyString_ShouldThrowNullRefferenceException()
        {
            // arrange
            var service = new LinkConvertorService();
            var expectedValue = "Value cannot be null. (Parameter 'uriString')";

            //act
            var result = Assert.Throws<ArgumentNullException>(() => service.GetDefaultSitemap(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Theory]
        [InlineData("https://www.ukad-group.com/", "https://www.ukad-group.com/")]
        [InlineData("https://www.facebook.com/ukadgroup/", "https://www.facebook.com/")]
        [InlineData("https://www.ukad-group.com/123/", "https://www.ukad-group.com/")]
        [InlineData("https://www.ukad-group.com/ukadgroup/?param1=0", "https://www.ukad-group.com/")]
        public void GetRootUrl_Url_ShouldReturnRootUrl(string url, string expectedValue)
        {
            // arrange
            var service = new LinkConvertorService();

            //act
            var result = service.GetRootUrl(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void GetRootUrl_Null_ShouldThrowNullRefferenceException()
        {
            // arrange
            var service = new LinkConvertorService();
            var expectedValue = "Invalid URI: The URI is empty.";

            //act
            var result = Assert.Throws<UriFormatException>(() => service.GetRootUrl(string.Empty));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Fact]
        public void GetRootUrl_EmptyString_ShouldThrowNullRefferenceException()
        {
            // arrange
            var service = new LinkConvertorService();
            var expectedValue = "Value cannot be null. (Parameter 'uriString')";

            //act
            var result = Assert.Throws<ArgumentNullException>(() => service.GetRootUrl(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }
    }
}
