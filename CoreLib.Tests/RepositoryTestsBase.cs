using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests
{
    public abstract class RepositoryTestsBase
    {
        protected TestDbContext _dbContext;

        public async Task InitializeAsync() => await InitializeData();
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

        protected async Task InitializeData()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            _dbContext = new TestDbContext(options);
            await _dbContext.MockItems.AddRangeAsync(DataList.MockItems);
            await _dbContext.SaveChangesAsync();
        }
    }
}