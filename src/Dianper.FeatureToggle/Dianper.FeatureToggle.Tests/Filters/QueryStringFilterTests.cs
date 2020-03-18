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

    public class QueryStringFilterTests : IDisposable
    {
        private readonly QueryStringFilter target;
        private readonly FeatureFilterEvaluationContext context;
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private const string FeatureName = "FeatureD";

        public QueryStringFilterTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.target = new QueryStringFilter(this.httpContextAccessorMock.Object);
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
        public async Task EvaluateAsync_WhenHasValidQueryString_ReturnsTrue()
        {
            // Arrange
            var queryStringValue = new StringValues("true");

            this.SetUpQueryString(FeatureName, queryStringValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.True(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Query.TryGetValue(FeatureName, out queryStringValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasNoValueOnQueryString_ReturnsFalse()
        {
            // Arrange
            var queryStringValue = default(StringValues);

            this.SetUpQueryString(FeatureName, queryStringValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Query.TryGetValue(FeatureName, out queryStringValue), Times.Once);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasInvalidQueryString_ReturnsFalse()
        {
            // Arrange
            var featureName = "FeatureX";
            var queryStringValue = new StringValues("true");

            this.SetUpQueryString(featureName, queryStringValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Query.TryGetValue(FeatureName, out queryStringValue), Times.Once);
        }

        private void SetUpQueryString(string key, StringValues value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Query.TryGetValue(key, out value))
                .Returns(true);
        }
    }
}
