namespace dianper.feature.toggle.web.Controllers
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
            var taskA = featureManager.IsEnabledAsync("FeatureA");
            var taskB = featureManager.IsEnabledAsync("FeatureB");
            var taskC = featureManager.IsEnabledAsync("FeatureC");
            var taskD = featureManager.IsEnabledAsync("FeatureD");

            var tasks = new List<Task<bool>>
            {
                taskA,
                taskB,
                taskC,
                taskD
            };

            await Task.WhenAll(tasks);

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
