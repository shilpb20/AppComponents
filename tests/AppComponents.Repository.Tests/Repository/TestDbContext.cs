using Microsoft.EntityFrameworkCore;

namespace AppComponents.Repository.Tests
{
    public class TestDbContext : DbContext
    {
        public DbSet<MockItem> MockItems { get; set; }
        public DbSet<TimeStampedMockItem> TimeStampedMockItems { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

    }
}
