namespace dianper.feature.toggle.web.Filters
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    public class SettingsFilter : IFeatureFilter
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return Task.FromResult(context.Parameters.Get<bool>());
        }
    }
}
