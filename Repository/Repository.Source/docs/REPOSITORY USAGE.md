## Work in Progress

### 1. Create the `Category` Entity and `DbContext`

First, create the `Category` entity, followed by the `ApplicationDbContext` to handle the database interaction.

public class Category
{
    public int Id { get; set; }  // Unique identifier for the category
    public string Name { get; set; }  // Name of the category
}

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Category> Categories { get; set; }  // Represents the Categories table in the database
}

### 2. Register Logging and DbContext

builder.Services.AddLogging();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

### 3. Register Repository

builder.Services.AddRepository<Category, ApplicationDbContext>();
