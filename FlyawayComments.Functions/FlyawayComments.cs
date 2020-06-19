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
using AutoMapper.QueryableExtensions;
using FlyawayComment.Functions.Models;
using AutoMapper;
using System.Collections.Generic;

namespace FlyawayComment.Functions
{
    public class FlyawayComments
    {
        private readonly IFlyawayRepository repo;
        private readonly IMapper mapper;
        public FlyawayComments(IFlyawayRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [FunctionName("FlyawayComments")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string date = req.Query["date"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            date = date ?? data?.date;

            DateTime dateAdded;

            var isDate = DateTime.TryParse(date, out dateAdded);
            if (!isDate)
                dateAdded = DateTime.Now;


            //this will query all fields and no mapping - so return all fields
            //var comments = repo.GetFlyawayComments(dateAdded).ToList();

            //this will get all fields from db first then map to whatever in LaxgroundTransportationDTO - return smaller no of fields in DTO
            //var comments = mapper.Map<List<LaxgroundTransportationDTO>>(repo.GetFlyawayComments(dateAdded));

            //most efficient: this will only query db for the fields specified in LaxgroundTransportationDTO - return smaller no of fields in DTO
            var comments = repo.GetFlyawayComments(dateAdded).ProjectTo<LaxgroundTransportationDTO>(mapper.ConfigurationProvider).ToList();

            return new OkObjectResult(comments);
        }
    }
}
