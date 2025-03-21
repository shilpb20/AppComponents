using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TestData = AppComponents.Repository.Tests.TimeStampedRepoTestData;

namespace AppComponents.Repository.Tests
{
    public abstract class TimeStampedRepositoryTestsBase
    {
        protected TestDbContext? _dbContext;

        protected readonly Expression<Func<TimeStampedMockItem, bool>> _queryNewItemByName = x => x.Name == TestData.NewItem.Name;
        protected readonly Expression<Func<TimeStampedMockItem, bool>> _queryItemWithId1 = x => x.Id == 1;
        protected readonly Expression<Func<TimeStampedMockItem, bool>> _queryItemForUpdateData = x => x.Id == 5;

        public async Task InitializeAsync()
        {
            await InitializeAsync(mockItems: TestData.MockItems);
        }

        public async Task InitializeAsync(IEnumerable<TimeStampedMockItem>? mockItems = null)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestDbContext(options);

            await _dbContext.Database.EnsureCreatedAsync();

            if (mockItems != null)
            {
                await _dbContext.TimeStampedMockItems.AddRangeAsync(mockItems);
            }

            await _dbContext.SaveChangesAsync();
        }

        // Dispose DbContext
        public async Task DisposeAsync() => await _dbContext.DisposeAsync().AsTask();

        protected void AssertMockItems(IEnumerable<TimeStampedMockItem> expectedItems, IEnumerable<TimeStampedMockItem> actualItems)
        {
            Assert.NotNull(actualItems);

            Assert.Collection(actualItems, expectedItems
                .Select(expected => (Action<TimeStampedMockItem>)(actual =>
                {
                    Assert.Equal(expected.Id, actual.Id);
                    Assert.Equal(expected.Name, actual.Name);
                    Assert.Equal(expected.Value, actual.Value);
                }))
                .ToArray());
        }

        protected void AssertMockItem(TimeStampedMockItem? expectedResult, TimeStampedMockItem? actualResult)
        {
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult?.Id, actualResult?.Id);
            Assert.Equal(expectedResult?.Name, actualResult?.Name);
            Assert.Equal(expectedResult?.Value, actualResult?.Value);
        }


        protected TimeStampedRepository<TimeStampedMockItem, TestDbContext> GetTimeStampedRepository()
        {
            if (_dbContext == null)
            {
                throw new ArgumentNullException($"Database object not initialized {0}", nameof(_dbContext));
            }

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            var logger = loggerFactory.CreateLogger<TimeStampedRepository<TimeStampedMockItem, TestDbContext>>();

            var repository = new TimeStampedRepository<TimeStampedMockItem, TestDbContext>(_dbContext, logger);
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository;
        }
    }
}