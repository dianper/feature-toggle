namespace Dianper.FeatureToggle.Sample.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.FeatureManagement;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IFeatureManager featureManager;

        public ValuesController(IFeatureManager featureManager)
        {
            this.featureManager = featureManager;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var taskA = this.featureManager.IsEnabledAsync("FeatureA");
            var taskB = this.featureManager.IsEnabledAsync("FeatureB");
            var taskC = this.featureManager.IsEnabledAsync("FeatureC");
            var taskD = this.featureManager.IsEnabledAsync("FeatureD");

            await Task.WhenAll(taskA, taskB, taskC, taskD);

            var result = new List<string>
            {
                $"FeatureA: { taskA.Result }",
                $"FeatureB: { taskB.Result }",
                $"FeatureC: { taskC.Result }",
                $"FeatureD: { taskD.Result }"
            };

            return result.ToArray();
        }
    }
}
