using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests
{
    public class UrlValidationServiceTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData("https://www.ukad-group.com/123", true)]
        [InlineData("https://www.facebook.com/ukadgroup",false)]
        public void ContainsBaseUrl_Url_ShouldReturnBooleanValue(string url, bool expectedValue)
        {
            // arrange
            var service = new UrlValidatorService();
            var baseUrl = "https://www.ukad-group.com/";

            //act
            var result = service.ContainsBaseUrl(url, baseUrl);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void ContainsBaseUrl_Null_ShouldThrowNullRefferenceException()
        {
            // arrange

            var service = new UrlValidatorService();
            var baseUrl = "https://www.ukad-group.com/";
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => service.ContainsBaseUrl(null, baseUrl));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

        [Theory]
        [InlineData(null,false)]
        [InlineData("", false)]
        [InlineData("https://www.ukad-group.com/123/", true)]
        [InlineData("https://www.facebook.com/ukadgroup/", true)]
        [InlineData("www.ukad-group.com/123/", false)]
        [InlineData("/123/", false)]
        public void IsCorrectLink_Url_ShouldReturnBooleanValue(string url, bool expectedValue)
        {
            // arrange
            var service = new UrlValidatorService();

            //act
            var result = service.IsCorrectLink(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("https://www.ukad-group.com/123/", true)]
        [InlineData("https://www.facebook.com/ukadgroup/", true)]
        [InlineData("www.ukad-group.com/123/", true)]
        [InlineData("www.ukad-group.com/123.png", false)]
        [InlineData("www.ukad-group.com/123.jpeg", false)]
        [InlineData("www.ukad-group.com/123.jpg", false)]
        [InlineData("www.ukad-group.com/123.ico", false)]
        [InlineData("www.ukad-group.com/123.woff", false)]
        [InlineData("www.ukad-group.com/123.woff2", false)]
        [InlineData("www.ukad-group.com/123.js", false)]
        [InlineData("www.ukad-group.com/123.css", false)]
        [InlineData("www.ukad-group.com/123.json", false)]
        [InlineData("www.ukad-group.com/123.git", false)]
        [InlineData("www.ukad-group.com/123.ttf", false)]
        [InlineData("www.ukad-group.com/123?p=1", false)]
        [InlineData("www.ukad-group.com/123#p123", false)]
        [InlineData("mailto:hi@ukad-group.com", false)]
        [InlineData("/123/", true)] 
        public void IsValidLink_Url_ShouldReturnBooleanValue(string url, bool expectedValue)
        {
            // arrange
            var service = new UrlValidatorService();

            //act
            var result = service.IsValidLink(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void IsValidLink_Null_ShouldThrowNullRefferenceException()
        {
            // arrange

            var service = new UrlValidatorService();
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => service.IsValidLink(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

    }
}
