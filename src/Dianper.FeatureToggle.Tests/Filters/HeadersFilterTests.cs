namespace Dianper.FeatureToggle.Tests.Filters
{
    using Dianper.FeatureToggle.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.FeatureManagement;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class HeadersFilterTests : IDisposable
    {
        private readonly HeadersFilter target;
        private readonly FeatureFilterEvaluationContext context;
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private const string FeatureName = "FeatureD";

        public HeadersFilterTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.target = new HeadersFilter(this.httpContextAccessorMock.Object);
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
        public async Task EvaluateAsync_WhenHasValidHeaders_ReturnsTrue()
        {
            // Arrange
            var headersValue = new StringValues("true");

            this.SetUpHeaders(FeatureName, headersValue);

            // Act
            var result = await this.target.EvaluateAsync(this.context);

            // Assert
            Assert.True(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers.TryGetValue(FeatureName, out headersValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasNoValueOnHeaders_ReturnsFalse()
        {
            // Arrange
            var headersValue = default(StringValues);

            this.SetUpHeaders(FeatureName, headersValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers.TryGetValue(FeatureName, out headersValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasInvalidHeaders_ReturnsFalse()
        {
            // Arrange
            var featureName = "featureX";
            var headersValue = new StringValues("true");

            this.SetUpHeaders(featureName, headersValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers.TryGetValue(FeatureName, out headersValue), Times.Once);
        }

        private void SetUpHeaders(string key, StringValues value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Headers.TryGetValue(key, out value))
                .Returns(true);
        }
    }
}
