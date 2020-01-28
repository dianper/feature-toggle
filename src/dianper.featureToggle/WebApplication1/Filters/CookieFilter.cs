namespace dianper.feature.toggle.web.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("Cookie")]
    public class CookieFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            if (this.httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(context.FeatureName, out var cookieValue) &&
                !string.IsNullOrEmpty(cookieValue) &&
                bool.TryParse(cookieValue, out var result))
            {
                return Task.FromResult(result);
            }

            return Task.FromResult(false);
        }
    }
}
