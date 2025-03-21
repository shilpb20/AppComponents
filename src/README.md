# AppComponents.CoreLib.Repository Documentation

## Overview
`AppComponents.CoreLib.Repository` is a generic repository implementation for Entity Framework Core, providing reusable data access patterns including CRUD operations, filtering, ordering, and pagination.

## Features
- Generic repository pattern
- Async CRUD operations
- Filtering using `Expression<Func<T, bool>>`
- Dynamic ordering
- Pagination support
- Logging for entity operations
- Fully compatible with ASP.NET Core

## Installation

Install via NuGet Package Manager:

```sh
Install-Package AppComponents.Repository
```

Or via .NET CLI:

```sh
dotnet add package AppComponents.Repository
```

## Getting Started
### 1. Configure DbContext
Ensure you have an `AppDbContext` that inherits from `DbContext`.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
```

### 2. Define Entity
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```

### 3. Register Repository in Dependency Injection
```csharp
services.AddDbContext<AppDbContext>(options => options.UseSqlServer("YourConnectionString"));
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

### 4. Using Repository
#### Add Entity
```csharp
var userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
await userRepository.AddAsync(new User { Name = "Alice", Age = 30 });
```

#### Get All Entities with Filtering and Ordering
```csharp
var users = await userRepository.GetAllAsync(
    filter: u => u.Age > 25,
    orderByClause: new Dictionary<string, bool> { { "Name", true } }
);
```

#### Pagination
```csharp
var paginatedUsers = await userRepository.GetAllAsync(paginationSpec: new Pagination(pageIndex: 1, pageSize: 10));
```

#### Update Entity
```csharp
var user = await userRepository.GetAsync(u => u.Id == 1);
if (user != null)
{
    user.Name = "Updated Name";
    await userRepository.UpdateAsync(user);
}
```

#### Delete Entity
```csharp
await userRepository.DeleteAsync(user);
```

## Contributing
1. Fork the repository
2. Create a new branch (`feature/my-feature`)
3. Commit your changes
4. Push and create a pull request

## License
MIT License.

