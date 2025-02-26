using Microsoft.EntityFrameworkCore;
using Repository.Tests.Repository.TestContext;
using Repository.Tests.TimeStampedRepository.TestContext;

namespace Repository.Tests.Repository
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
