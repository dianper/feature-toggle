namespace Dianper.FeatureToggle.Tests.Filters
{
    using Dianper.FeatureToggle.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Microsoft.FeatureManagement;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class SettingsFilterTests : IDisposable
    {
        private readonly SettingsFilter target;
        private readonly FeatureFilterEvaluationContext context;
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly Mock<IConfigurationSection> configurationSectionMock;
        private const string ParameterDefaultName = "Default";
        private const string ParameterDefaultValue = "true";
        private const string ParameterForceTurnOffName = "ForceTurnOff";

        public SettingsFilterTests()
        {
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.configurationSectionMock = new Mock<IConfigurationSection>();
            this.configurationMock = new Mock<IConfiguration>();

            this.SetUpConfiguration();

            this.target = new SettingsFilter(this.httpContextAccessorMock.Object);
            this.context = new FeatureFilterEvaluationContext
            {
                Parameters = this.configurationMock.Object,
            };
        }

        public void Dispose()
        {
            this.httpContextAccessorMock.VerifyNoOtherCalls();
            this.configurationMock.VerifyNoOtherCalls();
            this.configurationSectionMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasNoForceTurnOff_ReturnsTrue()
        {
            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.True(result);

            this.configurationSectionMock
                .Verify(v => v.Value, Times.Once);
            this.configurationMock
                .Verify(v => v.GetSection(ParameterDefaultName), Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext, Times.Exactly(3));
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasForceTurnOffByCookies_Returnsfalse()
        {
            // Arrange
            var cookieValue = "true";

            this.SetUpCookies(ParameterForceTurnOffName, cookieValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.configurationMock
                .Verify(v => v.GetSection(ParameterDefaultName), Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Cookies.TryGetValue(ParameterForceTurnOffName, out cookieValue), Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers, Times.Never);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Query, Times.Never);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasForceTurnOffByHeaders_Returnsfalse()
        {
            // Arrange
            var headersValue = new StringValues("true");

            this.SetUpHeaders(ParameterForceTurnOffName, headersValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.VerifyCookiesDidNotReturn();

            this.configurationMock
                .Verify(v => v.GetSection(ParameterDefaultName), Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers.TryGetValue(ParameterForceTurnOffName, out headersValue), Times.Once);
            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Query, Times.Never);
        }

        [Fact]
        public async Task EvaluateAsync_WhenHasForceTurnOffByQueryString_Returnsfalse()
        {
            // Arrange
            var queryStringValue = new StringValues("true");

            this.SetUpQueryString(ParameterForceTurnOffName, queryStringValue);

            // Act
            var result = await this.target.EvaluateAsync(context);

            // Assert
            Assert.False(result);

            this.VerifyCookiesDidNotReturn();
            this.VerifyHeadersDidNotReturn();

            this.configurationMock
                .Verify(v => v.GetSection(ParameterDefaultName), Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Query.TryGetValue(ParameterForceTurnOffName, out queryStringValue), Times.Once);
        }

        private void SetUpConfiguration()
        {
            this.configurationSectionMock
                .Setup(s => s.Value)
                .Returns(ParameterDefaultValue);
            this.configurationMock
                .Setup(s => s.GetSection(ParameterDefaultName))
                .Returns(this.configurationSectionMock.Object);
        }

        private void SetUpCookies(string key, string value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Cookies.TryGetValue(key, out value))
                .Returns(true);
        }

        private void SetUpHeaders(string key, StringValues value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Headers.TryGetValue(key, out value))
                .Returns(true);
        }

        private void SetUpQueryString(string key, StringValues value)
        {
            this.httpContextAccessorMock
                .Setup(s => s.HttpContext.Request.Query.TryGetValue(key, out value))
                .Returns(true);
        }

        private void VerifyCookiesDidNotReturn()
        {
            var cookiesValue = It.IsAny<string>();

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Cookies, Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Cookies.TryGetValue(It.IsAny<string>(), out cookiesValue), Times.Never);
        }

        private void VerifyHeadersDidNotReturn()
        {
            var headersValue = It.IsAny<StringValues>();

            this.httpContextAccessorMock
                .VerifyGet(v => v.HttpContext.Request.Headers, Times.Once);
            this.httpContextAccessorMock
                .Verify(v => v.HttpContext.Request.Headers.TryGetValue(It.IsAny<string>(), out headersValue), Times.Never);
        }
    }
}
