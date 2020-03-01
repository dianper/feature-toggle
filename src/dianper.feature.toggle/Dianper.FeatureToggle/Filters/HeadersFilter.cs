namespace Dianper.FeatureToggle.Filters
{
    using Dianper.FeatureToggle.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System;
    using System.Threading.Tasks;

    [FilterAlias("Headers")]
    public class HeadersFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HeadersFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(this.httpContextAccessor.GetFromHeaders(context.FeatureName));
        }
    }
}
