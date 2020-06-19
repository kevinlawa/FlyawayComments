# Project Setup

- Create FlyawayComments.Data - _Need to create this project first because scaffolding dbcontext using reverse engineering technique will throw an error._

  - Add as class library project
  - [Reference EF Core SQL Server Package]("https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer")

        Install-Package Microsoft.EntityFrameworkCore.SqlServer

  - EF Core Database-First Reverse Engineering

        Install-Package Microsoft.EntityFrameworkCore.Tools

        Scaffold-DbContext "SqlConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables LaxgroundTransportation

    - Remove the method containing the hard-coded connection string in the file genereated by the scaffolding. The connection string will be passed in when the dbcontext is registered as a service (using dependency injection) in the Startup class of "FlyawayComments.Functions" project.

  - Use Repository Pattern
    - Create Interface ILaxRespository
    - Create LaxRepository Implementing ILaxRepository
    - Use Dependency Injection to inject ILaxRepository in Controller of FlyawayComments.Functions project

* Create FlyawayComments.Functions

  - [Reference EF Core SQL Server Package]("https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer")

        Install-Package Microsoft.EntityFrameworkCore.SqlServer

- [Reference Microsoft.Azure.Functions.Extensions]("https://www.nuget.org/packages/Microsoft.Azure.Functions.Extensions/")

        Install-Package Microsoft.Azure.Functions.Extensions

- [Reference Microsoft.NET.Sdk.Functions]("https://www.nuget.org/packages/Microsoft.NET.Sdk.Functions/")

      Install-Package Microsoft.NET.Sdk.Functions

- [Reference AutoMapper]("https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection") - _Optional but recommended_

        Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection

  - > Create Automapper Mapping by inheriting the Profile Class

          public class AutoMapping : Profile
          {
              public AutoMapping()
              {
                  //Only want some fields and not all the fields from the table
                  CreateMap<LaxgroundTransportation, LaxgroundTransportationDTO>();
              }
          }

  - > Register Automapper as a service

          public class Startup : FunctionsStartup
          {
              public override void Configure(IFunctionsHostBuilder builder)
              {
                  //register automapper
                  builder.Services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
              }
          }

  - > Inject automapper via constructor

          private readonly IMapper mapper;
          public FlyawayComments(IMapper mapper)
          {
              this.mapper = mapper;
          }

- Reference FlyawayComments.Data Project

- Create Startup class to register services to do dependency injection

        using System;
        using AutoMapper;
        using FlyawayComments.Data.Models;
        using FlyawayComment.Functions.Models;
        using FlyawayComments.Data.Repositories;
        using Microsoft.Azure.Functions.Extensions.DependencyInjection;
        using Microsoft.EntityFrameworkCore;
        using Microsoft.Extensions.DependencyInjection;

        [assembly: FunctionsStartup(typeof(FlyawayComment.Functions.Startup))]

        namespace FlyawayComment.Functions
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

  - Using constructor dependency injection

          private readonly IFlyawayRepository repo;
          private readonly IMapper mapper;
          public FlyawayComments(IFlyawayRepository repo, IMapper mapper)
          {
              this.repo = repo;
              this.mapper = mapper;
          }

  - Remove all static keywords in the Azure Function Class(es) because constructor dependency injection requires concreate class to contruct with the use of the constructor.