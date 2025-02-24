using Microsoft.EntityFrameworkCore;

namespace Repository.Tests.Data
{
    public class TestDbContext : DbContext
    {
        public DbSet<MockItem> MockItems { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

    }
}
