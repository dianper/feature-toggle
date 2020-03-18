namespace Dianper.FeatureToggle.Tests.Filters
{
    using Dianper.FeatureToggle.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class CookiesFilterTests : IDisposable
    {
        private readonly CookiesFilter target;
        private readonly FeatureFilterEvaluationContext context;
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private const string FeatureName = "FeatureD";

        public CookiesFilterTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.target = new CookiesFilter(this.httpContextAccessorMock.Object);
            this.context = new FeatureFilterEvaluationContext
            {
                FeatureName = FeatureName
            };
        }

        public void Dispose()
        {
            this.httpContextAccessorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasValidCookies_ReturnsTrue()
        {
            // Arrange
            var cookiesValue = "true";

            this.SetUpCookies(FeatureName, cookiesValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.True(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Cookies.TryGetValue(FeatureName, out cookiesValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasNoValueOnCookies_ReturnsFalse()
        {
            // Arrange
            var cookieValue = default(string);

            this.SetUpCookies(FeatureName, cookieValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Cookies.TryGetValue(FeatureName, out cookieValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasInvalidCookies_ReturnsFalse()
        {
            // Arrange
            var featureName = "FeatureX";
            var cookiesValue = "true";

            this.SetUpCookies(featureName, cookiesValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Cookies.TryGetValue(FeatureName, out cookiesValue), Times.Once);
        }

        private void SetUpCookies(string key, string value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Cookies.TryGetValue(key, out value))
                .Returns(true);
        }
    }
}
