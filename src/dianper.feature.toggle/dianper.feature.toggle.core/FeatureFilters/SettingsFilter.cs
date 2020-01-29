namespace dianper.feature.toggle.core.FeatureFilters
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("Settings")]
    public class SettingsFilter : IFeatureFilter
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(context.Parameters.Get<bool>());
        }
    }
}
