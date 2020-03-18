namespace Dianper.FeatureToggle.Tests.Extensions
{
    using Dianper.FeatureToggle.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using System;
    using Xunit;

    public class HttpContextAccessorExtensionsTests : IDisposable
    {
        private const string FeatureName = "feature";
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private readonly Mock<IRequestCookieCollection> requestCookieCollectionMock;
        private readonly Mock<IHeaderDictionary> headerDictionaryMock;
        private readonly Mock<IQueryCollection> queryCollectionMock;

        public HttpContextAccessorExtensionsTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.requestCookieCollectionMock = new Mock<IRequestCookieCollection>();
            this.headerDictionaryMock = new Mock<IHeaderDictionary>();
            this.queryCollectionMock = new Mock<IQueryCollection>();
        }

        public void Dispose()
        {
            this.httpContextAccessorMock.VerifyNoOtherCalls();
            this.requestCookieCollectionMock.VerifyNoOtherCalls();
            this.headerDictionaryMock.VerifyNoOtherCalls();
            this.queryCollectionMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("cookies")]
        [InlineData("headers")]
        [InlineData("querystring")]
        public void WhenHttpAccessorIsNull_ReturnsFalse(string from)
        {
            // Act
            var result = this.Execute(null, from);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("cookies")]
        [InlineData("headers")]
        [InlineData("querystring")]
        public void WhenHttpContextNull_ReturnsFalse(string from)
        {
            // Arrange
            var target = this.httpContextAccessorMock.Object;

            // Act
            var result = this.Execute(target, from);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext, Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request, Times.Never);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("abc", false)]
        [InlineData("123", false)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void GetFromCookies_WhenCookieExists_ReturnsExpectedValue(string cookieValue, bool expectedValue)
        {
            // Arrange
            this.httpContextAccessorMock
                .SetupGet(s => s.HttpContext.Request.Cookies)
                .Returns(this.requestCookieCollectionMock.Object);

            this.requestCookieCollectionMock
                .Setup(s => s.TryGetValue(FeatureName, out cookieValue))
                .Returns(true);

            var target = this.httpContextAccessorMock.Object;

            // Act
            var result = target.GetFromCookies(FeatureName);

            // Assert
            Assert.Equal(expectedValue, result);

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext, Times.Once);
            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Cookies, Times.Once);
            this.requestCookieCollectionMock
                .Verify(v => v.TryGetValue(FeatureName, out cookieValue), Times.Once);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("abc", false)]
        [InlineData("123", false)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void GetFromHeaders_WhenHeadersExists_ReturnsExpectedValue(string value, bool expectedValue)
        {
            // Arrange
            this.httpContextAccessorMock
                .SetupGet(s => s.HttpContext.Request.Headers)
                .Returns(this.headerDictionaryMock.Object);

            var headersValue = new StringValues(value);
            this.headerDictionaryMock
                .Setup(s => s.TryGetValue(FeatureName, out headersValue))
                .Returns(true);

            var target = this.httpContextAccessorMock.Object;

            // Act
            var result = target.GetFromHeaders(FeatureName);

            // Assert
            Assert.Equal(expectedValue, result);

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext, Times.Once);
            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Headers, Times.Once);
            this.headerDictionaryMock
                .Verify(v => v.TryGetValue(FeatureName, out headersValue), Times.Once);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("abc", false)]
        [InlineData("123", false)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void GetFromQueryString_WhenQueryStringExists_ReturnsExpectedValue(string value, bool expectedValue)
        {
            // Arrange
            this.httpContextAccessorMock
                .SetupGet(s => s.HttpContext.Request.Query)
                .Returns(this.queryCollectionMock.Object);

            var queryValue = new StringValues(value);
            this.queryCollectionMock
                .Setup(s => s.TryGetValue(FeatureName, out queryValue))
                .Returns(true);

            var target = this.httpContextAccessorMock.Object;

            // Act
            var result = target.GetFromQueryString(FeatureName);

            // Assert
            Assert.Equal(expectedValue, result);

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext, Times.Once);
            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Query, Times.Once);
            this.queryCollectionMock
                .Verify(v => v.TryGetValue(FeatureName, out queryValue), Times.Once);
        }

        private bool Execute(IHttpContextAccessor target, string from)
        {
            if (from.Equals("cookies"))
            {
                return target.GetFromCookies(FeatureName);
            }
            else if (from.Equals("headers"))
            {
                return target.GetFromHeaders(FeatureName);
            }
            else if (from.Equals("querystring"))
            {
                return target.GetFromQueryString(FeatureName);
            }

            return false;
        }
    }
}
