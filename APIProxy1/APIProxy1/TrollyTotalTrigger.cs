using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using APIProxy1.Services;
using APIProxy1.Models;

namespace APIProxy1
{
    public class TrolleyTotalTrigger
    {
        private readonly IResourceService _service;

        public TrolleyTotalTrigger(IResourceService service)
        {
            this._service = service;
        }

        [FunctionName("TrolleyTotal")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TrolleyModel data = JsonConvert.DeserializeObject<TrolleyModel>(requestBody);
            var products = data.products;
            var specials = data.specials;
            var quantities = data.quantities;


            var res = _service.TrolleyTotal(products, specials, quantities);     

            return new OkObjectResult(res);           
        }
    }
}
