# Repository Usage

This guide explains how to use the **AppComponents.Repository** NuGet package to integrate the **Repository Pattern** and **Transaction Management** in your application.

## 1. Installation

To get started, first install the `AppComponents.Repository` package via NuGet:

```bash
dotnet add package AppComponents.Repository
```

## 2. Configuring appsettings.json
You can configure the database and transaction settings in your appsettings.json file as below. 

TransactionSettings: Optional settings for using transactions.
Set UseInMemoryDatabase to:
   - 'true' for testing with an in-memory database, typically for testing, 
   - 'false' for a real database connection.


```json
{
  "ConnectionStrings": {
    "YourDatabase": "Your SQL Server connection string here"
  },
  "TransactionSettings": {
    "UseInMemoryDatabase": false
  }
}
```

## 3. Registering Services in Program.cs
To use the repository and transaction manager, you'll need to register them in your Program.cs or Startup.cs file. Here's how you can do it:

```csharp
var builder = WebApplication.CreateBuilder(args);

//Read Transaction Settings
var transactionSettings = builder.Configuration.GetSection("TransactionSettings");

// Add your custom DbContext (ApplicationDbContext) here, which should be defined elsewhere in your application.
//Instantiate DbContext with actual database or InMemory database for testing purpose. (value configured in TransactionSettings)
bool useInMemoryDb = transactionSettings.GetValue<bool>("UseInMemoryDatabase"); 
if (useInMemoryDb)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDB"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("YourDatabase")));
}

// Register repositories and transaction manager
//Replace YourEntity and ApplicationDbContext with your actual entity and context
builder.Services.AddRepository<YourEntity, ApplicationDbContext>();
builder.Services.AddTransactionManager<ApplicationDbContext>(builder.Configuration);

//Use time stamped repository if you want to capture Modified Time on entity update.
builder.Services.AddTimeStampedRepository<YourEntity, ApplicationDbContext>();


// Register your services
builder.Services.AddScoped<YourEntityService>();

var app = builder.Build();
app.Run();
```

## 4. Use Repository service in your service
```csharp
// Your service that uses the repository
public class YourEntityService
{
    private readonly IRepository<YourEntity, ApplicationDbContext> _repository;

    public YourEntityService(IRepository<YourEntity, ApplicationDbContext> repository)
    {
        _repository = repository;
    }

        // TBD: Add your methods for CRUD operations or business logic here.

}
```