using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using APIProxy1.Models;
using APIProxy1.Services;

namespace APIProxy1
{
    public class UserTrigger
    {
        private readonly IResourceService _service;

        public UserTrigger(IResourceService service)
        {
            this._service = service;
        }

        [FunctionName("User")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Answers/user")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var res = new UserModel { name = "test", token = "1234-455662-22233333-3333" };

            return new OkObjectResult(res);
        }
    }
}
