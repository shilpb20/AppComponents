using AppComponents.CoreLib;
using CoreLib.Tests;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace CoreLib.Tests
{
    public abstract class RepositoryTestsBase
    {
        protected TestDbContext? _dbContext;

        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId0 = x => x.Id == 0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId1 = x => x.Id == 1;

        protected readonly Expression<Func<MockItem, bool>> _queryItemWitDuplicateName = x => x.Name == TestData.DuplicateName;
        protected readonly Expression<Func<MockItem, bool>> _queryNewItemByName = x => x.Name == TestData.NewItem.Name;

        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithEvenId = x => x.Id % 2 ==0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithOddId = x => x.Id % 2 != 0;


        protected readonly QueryTrackingBehavior _trackAll = QueryTrackingBehavior.TrackAll;
        protected readonly QueryTrackingBehavior _noTrack = QueryTrackingBehavior.NoTracking;

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

            if(mockItems != null)
            {
                await _dbContext.MockItems.AddRangeAsync(mockItems);
            }

            await _dbContext.SaveChangesAsync();
        }

        // Dispose DbContext
        public Task DisposeAsync() => _dbContext.DisposeAsync().AsTask();

        protected void AssertMockItemsEqual(IEnumerable<MockItem> expectedItems, IEnumerable<MockItem> actualItems)
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

        protected void AssertMockItem(MockItem? expectedResult, MockItem? actualResult)
        {
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult?.Id, actualResult?.Id);
            Assert.Equal(expectedResult?.Name, actualResult?.Name);
        }

        protected void AssertTrackingBehavior(QueryTrackingBehavior expectedBehavior)
        {
            var result = _dbContext?.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(expectedBehavior, result);
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