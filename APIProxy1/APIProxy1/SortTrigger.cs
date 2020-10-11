using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using APIProxy1.Models;
using APIProxy1.Services;
using Microsoft.Extensions.Options;

namespace APIProxy1
{
    public class SortTrigger
    {
        private readonly IResourceService _service;

        public SortTrigger(IResourceService service)
        {
            this._service = service;
        }

        [FunctionName("Sort")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            SortType sort = SortType.Recommended;

            if (!string.IsNullOrEmpty(req.Query["sortOption"]))
            {
                if (!Enum.TryParse(req.Query["sortOption"], out sort))
                {
                    return new BadRequestObjectResult(string.Format("The specified sort type {0} is not valid.", req.Query["sortOption"]));
                }
            }

            var res = await _service.GetShopperHistorySortedBy(sort);     

            return new OkObjectResult(res);           
        }
    }
}
