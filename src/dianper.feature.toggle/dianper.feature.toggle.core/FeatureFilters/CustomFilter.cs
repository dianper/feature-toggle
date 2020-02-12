namespace dianper.feature.toggle.core.FeatureFilters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.FeatureManagement;
    using System.Threading.Tasks;

    [FilterAlias("Custom")]
    public class CustomFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var subFolder = context.Parameters.GetValue<string>("subFolder");

            if (this.httpContextAccessor.HttpContext.Request.Query.TryGetValue("subFolder", out var queryStringValue) &&
                !string.IsNullOrEmpty(queryStringValue))
            {
                if (subFolder.Contains(queryStringValue))
                {
                    return Task.FromResult(true);
                }
            }           

            return Task.FromResult(false);
        }
    }
}
