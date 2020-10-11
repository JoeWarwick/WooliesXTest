using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using APIProxy1.Services;

namespace APIProxy1
{
    public class AnswersTrigger
    {
        private readonly IResourceService _service;

        public AnswersTrigger(IResourceService service)
        {
            this._service = service;
        }

        [FunctionName("Answers")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var res = _service.GetUser(req.RequestUri.ToString().Split('?')[0], log);     

            return new OkObjectResult(res);
        }
    }
}
