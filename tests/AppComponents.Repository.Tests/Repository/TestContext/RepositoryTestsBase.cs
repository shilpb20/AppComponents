using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TestData = AppComponents.Repository.Tests.RepoTestData;

namespace AppComponents.Repository.Tests
{
    public abstract class RepositoryTestsBase
    {
        protected TestDbContext? _dbContext;

        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithPositiveIds = x => x.Id > 0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithNegativeIds = x => x.Id < 0;

        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId0 = x => x.Id == 0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId1 = x => x.Id == 1;

        protected readonly Expression<Func<MockItem, bool>> _queryItemForUpdateData = x => x.Id == 5;

        protected readonly Expression<Func<MockItem, bool>> _queryItemWitDuplicateName = x => x.Name == TestData.DuplicateName;
        protected readonly Expression<Func<MockItem, bool>> _queryNewItemByName = x => x.Name == TestData.NewItem.Name;

        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithEvenId = x => x.Id % 2 == 0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithOddId = x => x.Id % 2 != 0;


        public async Task InitializeAsync()
        {
            await InitializeAsync(mockItems: TestData.MockItems);
        }

        public async Task InitializeAsync(IEnumerable<MockItem>? mockItems = null)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TestDbContext(options);

            await _dbContext.Database.EnsureCreatedAsync();

            if (mockItems != null)
            {
                await _dbContext.MockItems.AddRangeAsync(mockItems);
            }

            await _dbContext.SaveChangesAsync();
        }

        // Dispose DbContext
        public async Task DisposeAsync() => await _dbContext.DisposeAsync().AsTask();

        protected void AssertMockItems(IEnumerable<MockItem> expectedItems, IEnumerable<MockItem> actualItems)
        {
            Assert.NotNull(actualItems);

            Assert.Collection(actualItems, expectedItems
                .Select(expected => (Action<MockItem>)(actual =>
                {
                    Assert.Equal(expected.Id, actual.Id);
                    Assert.Equal(expected.Name, actual.Name);
                    Assert.Equal(expected.Value, actual.Value);
                }))
                .ToArray());
        }

        protected void AssertMockItem(MockItem? expectedResult, MockItem? actualResult)
        {
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult?.Id, actualResult?.Id);
            Assert.Equal(expectedResult?.Name, actualResult?.Name);
            Assert.Equal(expectedResult?.Value, actualResult?.Value);
        }

        protected Repository<MockItem, TestDbContext> GetRepository()
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

            var logger = loggerFactory.CreateLogger<Repository<MockItem, TestDbContext>>();

            var repository = new Repository<MockItem, TestDbContext>(_dbContext, logger);
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            return repository;
        }
    }
}