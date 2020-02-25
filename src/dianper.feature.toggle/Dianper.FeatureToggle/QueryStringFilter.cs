namespace Dianper.FeatureToggle
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("QueryString")]
    public class QueryStringFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public QueryStringFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            if (this.httpContextAccessor.HttpContext.Request.Query.TryGetValue(context.FeatureName, out var queryStringValue) &&
                !string.IsNullOrEmpty(queryStringValue) &&
                bool.TryParse(queryStringValue, out var result))
            {
                return Task.FromResult(result);
            }

            return Task.FromResult(false);
        }
    }
}
