using System;
using ClassLibrary1.Models;
using FlyawayComments.Data.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(FlyawayComment.Functions.Startup))]

namespace FlyawayComment.Functions
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("FlyawayConnectionString");
            builder.Services.AddDbContext<lawasitecore91prodexternaldbContext>(options =>
            {
                SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString);
            });

            builder.Services.AddScoped<IFlyawayRepository, FlyawayRepository>();

        }

    }
}
