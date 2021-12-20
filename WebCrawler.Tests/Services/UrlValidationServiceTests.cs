using System;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests.Services
{
    public class UrlValidationServiceTests
    {
        private readonly UrlValidatorService _service = new UrlValidatorService();


        [Theory]
        [InlineData("https://www.example.com/123/", true)]
        [InlineData("https://www.fakebook.com/somegroup/", true)]
        public void UrlIsValid_Url_ShouldReturnTrue(string stringUrl, bool expectedValue)
        {
            // arrange
            var url = new Uri(stringUrl);

            //act
            var result = _service.ValidateUrl(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("https://www.example.com/123/", true)]
        [InlineData("https://www.fakebook.com/somegroup/", true)]
        public void LinkIsValid_Url_ShouldReturnTrue(string stringUrl, bool expectedValue)
        {
            // arrange
            var url = new Uri(stringUrl);

            //act
            var result = _service.ValidateLink(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void LinkIsValid_Url_ShouldReturnFalse()
        {
            // arrange
            var url = new Uri("https://www.example.com/123.png");
            var expectedValue = false;
            //act
            var result = _service.ValidateLink(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void LinkIsValid_Null_ShouldThrowNullRefferenceException()
        {
            // arrange
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => _service.ValidateLink(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

    }
}
