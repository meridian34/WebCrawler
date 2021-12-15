using System;
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
        public void UrlIsValid_Url_ShouldReturnBooleanValue(string url, bool expectedValue)
        {
            // arrange
            var service = new UrlValidatorService();

            //act
            var result = service.UrlIsValid(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("https://www.ukad-group.com/123/", true)]
        [InlineData("https://www.facebook.com/ukadgroup/", true)]
        [InlineData("www.ukad-group.com/123/", true)]
        [InlineData("www.ukad-group.com/123.png", false)]
        [InlineData("www.ukad-group.com/123?p=1", false)]
        [InlineData("www.ukad-group.com/123#p123", false)]
        [InlineData("mailto:hi@ukad-group.com", false)]
        [InlineData("/123/", true)] 
        public void LinkIsValid_Url_ShouldReturnBooleanValue(string url, bool expectedValue)
        {
            // arrange
            var service = new UrlValidatorService();

            //act
            var result = service.LinkIsValid(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void LinkIsValid_Null_ShouldThrowNullRefferenceException()
        {
            // arrange

            var service = new UrlValidatorService();
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => service.LinkIsValid(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

    }
}
