# 1. Projects Setup

## 1.1. Create "FlyawayComments.Data" Project

- Need to create this project first because scaffolding dbcontext using reverse engineering technique will throw an error if the azure function project was created first.

- Add an EF Core class library project and name it "FlyawayComments.Data"

- [Reference EF Core SQL Server Package](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)

  ```
  Install-Package Microsoft.EntityFrameworkCore.SqlServer
  ```

- EF Core Database-First Reverse Engineering

  ```
  Install-Package Microsoft.EntityFrameworkCore.Tools

  Scaffold-DbContext "SqlConnectionString" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables LaxgroundTransportation
  ```

  Remove the method containing the hard-coded connection string (security concern) in the file generated by the scaffolding. The connection string will be passed in when the dbcontext is registered as a service (using dependency injection) in the Startup class of "FlyawayComments.Functions" project.

- Use Repository Pattern

  - Create Interface ILaxRespository

  - Create LaxRepository Implementing ILaxRepository

  - Use Dependency Injection to inject ILaxRepository in Controller of FlyawayComments.Functions project

## 1.2. Create "FlyawayComments.Functions" Project

- Add an Azure Function V3 Project to the solution and name it "FlyawayComments.Functions"

- Reference FlyawayComments.Data Project

- [Reference EF Core SQL Server Package](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)

  ```
  Install-Package Microsoft.EntityFrameworkCore.SqlServer
  ```

- [Reference Microsoft.Azure.Functions.Extensions](https://www.nuget.org/packages/Microsoft.Azure.Functions.Extensions/)
  ```
  Install-Package Microsoft.Azure.Functions.Extensions
  ```
- [Reference Microsoft.NET.Sdk.Functions](https://www.nuget.org/packages/Microsoft.NET.Sdk.Functions/)
  ```
  Install-Package Microsoft.NET.Sdk.Functions
  ```
- [Reference AutoMapper](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection) - _Optional but recommended_

  ```
  Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
  ```

  - Create Automapper Mapping by inheriting the Profile Class
    ```csharp
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Only want some fields and not all the fields from the table
            CreateMap<LaxgroundTransportation, LaxgroundTransportationDTO>();
        }
    }
    ```
  - Register Automapper as a service
    ```csharp
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //register automapper
            builder.Services.AddAutoMapper(typeof(Startup));
        }
    }
    ```
  - Inject automapper via constructor
    ```csharp
    private readonly IMapper mapper;
    public FlyawayComments(IMapper mapper)
    {
        this.mapper = mapper;
    }
    ```
  - Using automapper

    ```csharp
    //this will get all fields from db first then map to whatever in LaxgroundTransportationDTO - return smaller no of fields in DTO
    //var comments = mapper.Map<List<LaxgroundTransportationDTO>>(repo.GetFlyawayComments(dateAdded));

    //most efficient: this will only query db for the fields specified in LaxgroundTransportationDTO - return smaller no of fields in DTO
    var comments = repo.GetFlyawayComments(dateAdded).ProjectTo<LaxgroundTransportationDTO>(mapper.ConfigurationProvider).ToList();
    ```

- Create Startup class to register services to do dependency injection

  ```csharp
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
              builder.Services.AddAutoMapper(typeof(Startup));

              //register our repo
              builder.Services.AddScoped<IFlyawayRepository, FlyawayRepository>();

          }

      }
  }
  ```

- Using constructor dependency injection
  ```csharp
  private readonly IFlyawayRepository repo;
  private readonly IMapper mapper;
  public FlyawayComments(IFlyawayRepository repo, IMapper mapper)
  {
      this.repo = repo;
      this.mapper = mapper;
  }
  ```
- Remove all static keywords in the Azure Function class(es) because constructor dependency injection can not have static class.

- References

  - [EF Core with Azure Functions](https://markheath.net/post/ef-core-di-azure-functions)

  - [Use dependency injection in .NET Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)

  - [EF Core](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)

  - [AutoMapper](https://www.codementor.io/@zedotech/how-to-using-automapper-on-asp-net-core-3-0-via-dependencyinjection-zq497lzsq)
