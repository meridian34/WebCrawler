using System;
using WebCrawler.Services;
using Xunit;

namespace WebCrawler.Tests.Services
{
    public class UrlValidationServiceTests
    {
        private readonly UrlValidatorService _service = new UrlValidatorService();


        [Theory]
        [InlineData("https://www.ukad-group.com/123/", true)]
        [InlineData("https://www.facebook.com/ukadgroup/", true)]
        public void UrlIsValid_Url_ShouldReturnTrue(string stringUrl, bool expectedValue)
        {
            // arrange
            var url = new Uri(stringUrl);

            //act
            var result = _service.UrlIsValid(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("https://www.ukad-group.com/123/", true)]
        [InlineData("https://www.facebook.com/ukadgroup/", true)]
        public void LinkIsValid_Url_ShouldReturnTrue(string stringUrl, bool expectedValue)
        {
            // arrange
            var url = new Uri(stringUrl);

            //act
            var result = _service.LinkIsValid(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void LinkIsValid_Url_ShouldReturnFalse()
        {
            // arrange
            var url = new Uri("https://www.ukad-group.com/123.png");
            var expectedValue = false;
            //act
            var result = _service.LinkIsValid(url);

            //assert
            Assert.Equal(result, expectedValue);
        }

        [Fact]
        public void LinkIsValid_Null_ShouldThrowNullRefferenceException()
        {
            // arrange
            var expectedValue = "Object reference not set to an instance of an object.";

            //act
            var result = Assert.Throws<NullReferenceException>(() => _service.LinkIsValid(null));

            //assert
            Assert.Equal(result.Message, expectedValue);
        }

    }
}
