namespace Dianper.FeatureToggle.Filters
{
    using Dianper.FeatureToggle.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System;
    using System.Threading.Tasks;

    [FilterAlias("QueryString")]
    public class QueryStringFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public QueryStringFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(this.httpContextAccessor.GetFromQueryString(context.FeatureName));
        }
    }
}
