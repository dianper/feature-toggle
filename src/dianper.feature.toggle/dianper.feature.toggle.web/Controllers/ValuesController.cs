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

            var tasks = new List<Task<bool>>
            {
                taskA,
                taskB,
                taskC
            };

            await Task.WhenAll(tasks);

            var result = new List<string>
            {
                $"FeatureA: { taskA.Result }",
                $"FeatureB: { taskB.Result }",
                $"FeatureC: { taskC.Result }"
            };

            return result.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
