using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests
{
    public abstract class RepositoryTestsBase
    {
        protected TestDbContext _dbContext;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();


            await _dbContext.MockItems.AddRangeAsync(DataList.MockItems);
            await _dbContext.SaveChangesAsync();
        }

        // Dispose DbContext
        public Task DisposeAsync() => _dbContext.DisposeAsync().AsTask();

        protected static void AssertMockItemsEqual(IEnumerable<MockItem> expectedItems, IEnumerable<MockItem> actualItems)
        {
            Assert.NotNull(actualItems);

            Assert.Collection(actualItems, expectedItems
                .Select(expected => (Action<MockItem>)(actual =>
                {
                    Assert.Equal(expected.Id, actual.Id);
                    Assert.Equal(expected.Name, actual.Name);
                }))
                .ToArray());
        }

        protected Repository<MockItem> GetRepository()
        {
            var repository = new Repository<MockItem>(_dbContext);
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository;
        }
    }
}