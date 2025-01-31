using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreLib.Tests
{
    public abstract class RepositoryTestsBase
    {
        protected TestDbContext? _dbContext;

        public async Task InitializeAsync()
        {
            await InitializeAsync(mockItems:DataList.MockItems);
        }

        public async Task InitializeAsync(IEnumerable<MockItem>? mockItems = null)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestDbContext(options);

            await _dbContext.Database.EnsureCreatedAsync();

            if(mockItems != null)
            {
                await _dbContext.MockItems.AddRangeAsync(mockItems);
            }

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
            if(_dbContext == null)
            {
                throw new ArgumentNullException($"Database object not initialized {0}", nameof(_dbContext));
            }

            var repository = new Repository<MockItem>(_dbContext);
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository;
        }
    }
}