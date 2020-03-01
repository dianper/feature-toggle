namespace Dianper.FeatureToggle.Filters
{
    using Dianper.FeatureToggle.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System;
    using System.Threading.Tasks;

    [FilterAlias("Cookies")]
    public class CookiesFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookiesFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(this.httpContextAccessor.GetFromCookies(context.FeatureName));
        }
    }
}
