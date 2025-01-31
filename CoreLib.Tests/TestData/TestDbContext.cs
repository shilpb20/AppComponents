using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests.TestData
{
    public class TestDbContext : DbContext
    {
        public DbSet<MockItem> MockItems { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

    }
}
