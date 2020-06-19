using System;
using AutoMapper;
using FlyawayComments.Data.Models;
using FlyawayComments.Functions.Models;
using FlyawayComments.Data.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FlyawayComments.Functions.Startup))]

namespace FlyawayComments.Functions
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //register dbcontext
            string connectionString = Environment.GetEnvironmentVariable("FlyawayConnectionString");
            builder.Services.AddDbContext<lawasitecore91prodexternaldbContext>(options =>
            {
                SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString);
            });

            //register automapper
            builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            //register our repo
            builder.Services.AddScoped<IFlyawayRepository, FlyawayRepository>();

        }

    }
}
