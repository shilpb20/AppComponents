# AppComponents.Repository

A generic repository pattern implementation for Entity Framework Core.

## Installation

Install via NuGet Package Manager:

```sh
Install-Package AppComponents.Repository
```

Or via .NET CLI:

```sh
dotnet add package AppComponents.Repository
```

## Features
- Generic repository pattern for Entity Framework Core.
- Supports CRUD operations.
- Query filtering, ordering, and pagination.
- Integration with dependency injection.

## Usage

### Register Repository in Dependency Injection

```csharp
using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<MyDbContext>(options => options.UseSqlServer("your_connection_string"));
services.AddRepository<MyEntity, MyDbContext>();
```

### Define Your DbContext and Entity

```csharp
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}
    public DbSet<MyEntity> MyEntities { get; set; }
}

public class MyEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Using the Repository

```csharp
using AppComponents.Repository.Abstraction;

public class MyService
{
    private readonly IRepository<MyEntity, MyDbContext> _repository;
    
    public MyService(IRepository<MyEntity, MyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task AddEntityAsync()
    {
        var entity = new MyEntity { Name = "Sample Entity" };
        await _repository.AddAsync(entity);
    }

    public async Task<List<MyEntity>> GetEntitiesAsync()
    {
        return await _repository.GetAllAsync();
    }
}
```

## Pagination Support

```csharp
var pagination = new Pagination(pageIndex: 1, pageSize: 10);
var pagedResults = await _repository.GetAllAsync(paginationSpec: pagination);
```

## License
This project is licensed under the MIT License.

