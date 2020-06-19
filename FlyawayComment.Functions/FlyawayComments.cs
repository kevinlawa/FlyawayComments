using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FlyawayComments.Data.Repositories;
using System.Linq;

namespace FlyawayComment.Functions
{
    public class FlyawayComments
    {
        private readonly IFlyawayRepository repo;
        public FlyawayComments(IFlyawayRepository repo)
        {
            this.repo = repo;
        }

        [FunctionName("FlyawayComments")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var comments = repo.GetFlyawayComments(DateTime.Now.AddDays(-30).Date).ToList();

            return new OkObjectResult(comments);
        }
    }
}
