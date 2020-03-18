namespace Dianper.FeatureToggle.Filters
{
    using Dianper.FeatureToggle.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("Settings")]
    public class SettingsFilter : IFeatureFilter
    {
        protected readonly IHttpContextAccessor httpContextAccessor;
        private const string ForceDisabledFeatureName = "ForceTurnOff";
        private const string DefaultName = "Default";

        public SettingsFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var defaultValue = context.Parameters.GetSection(DefaultName);
            var paramName = $"{context.FeatureName}{ForceDisabledFeatureName}";

            if (this.httpContextAccessor.GetFromCookies(paramName) ||
                this.httpContextAccessor.GetFromHeaders(paramName) ||
                this.httpContextAccessor.GetFromQueryString(paramName))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(defaultValue != null && bool.Parse(defaultValue.Value));
        }
    }
}
