namespace dianper.feature.toggle.web.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("Headers")]
    public class HeadersFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HeadersFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            if (this.httpContextAccessor.HttpContext.Request.Headers.TryGetValue(context.FeatureName, out var headersValue) &&
                !string.IsNullOrEmpty(headersValue) &&
                bool.TryParse(headersValue, out var result))
            {
                return Task.FromResult(result);
            }

            return Task.FromResult(false);
        }
    }
}
