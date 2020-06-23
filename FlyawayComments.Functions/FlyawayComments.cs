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
using FlyawayComments.Functions.Models;
using AutoMapper;
using System.Collections.Generic;

namespace FlyawayComments.Functions
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

            //v1 - this will query all fields and no mapping - so return all fields
            //con: query all fields from db first (performance issue)
            var comments = repo.GetFlyawayComments(dateAdded).ToList();

            //v2 - this will query only the selected fields but we are creating anonymous type
            //con: dont know the type so have to find out from the code 
            //var comments = repo.GetFlyawayComments(dateAdded).Select(c => new { 
            //    c.TransportId,
            //    c.ServiceType,
            //    c.ServiceSubType
            //}).ToList();

            //v3 - this will query only the selected fields with strongly typed
            //con: have to manually type the mapping - this could be a problem if we have a lot of places that need this data return
            //can copy and paste can lead to bugs
            //var comments = repo.GetFlyawayComments(dateAdded).Select(c => new LaxgroundTransportationDTO() {
            //    TransportId = c.TransportId,
            //    ServiceType =  c.ServiceType,
            //    ServiceSubType = c.ServiceSubType
            //}).ToList();

            //v4 - this will get all fields from db first then map to whatever in LaxgroundTransportationDTO - return smaller no of fields in DTO
            //con: query all fields from db first (performance issue)
            //var comments = mapper.Map<List<LaxgroundTransportationDTO>>(repo.GetFlyawayComments(dateAdded));

            //v5 most efficient: this will only query db for the fields specified in LaxgroundTransportationDTO - return smaller no of fields in DTO
            //con: have to learn automapper - reference and configure it.
            //var comments = repo.GetFlyawayComments(dateAdded).ProjectTo<LaxgroundTransportationDTO>(mapper.ConfigurationProvider).ToList();

            return new OkObjectResult(comments);
        }
    }
}
