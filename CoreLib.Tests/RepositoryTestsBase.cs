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

        protected readonly Expression<Func<MockItem, bool>> _queryItemsWithPositiveIds = x => x.Id > 0;

        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId0 = x => x.Id == 0;
        protected readonly Expression<Func<MockItem, bool>> _queryItemWithId1 = x => x.Id == 1;

        protected readonly Expression<Func<MockItem, bool>> _queryItemForUpdateData = x => x.Id == 5;

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

        protected async Task AssertTrackingBehaviorForGetAllAsync(
            Repository<MockItem> repository,
            Expression<Func<MockItem, bool>> filter,
            bool? asNoTracking = null)
        {
            if (asNoTracking.HasValue)
            {
                var result = await repository.GetAllAsync(filter, asNoTracking.Value);
            }
            else
            {
                var result = await repository.GetAsync(filter);
            }
            VerifyTrackingBehavior(asNoTracking);
        }

        protected async Task AssertTrackingBehaviorForGetAsync(
            Repository<MockItem> repository,
            Expression<Func<MockItem, bool>> filter,
            bool? asNoTracking = null)
        {
            _dbContext.ChangeTracker.Clear();


            if (asNoTracking.HasValue)
            {
                var result = await repository.GetAsync(filter, asNoTracking.Value);
            }
            else
            {
                var result = await repository.GetAsync(filter);
            }

            VerifyTrackingBehavior(asNoTracking);
        }

        private void VerifyTrackingBehavior(bool? asNoTracking)
        {
            var currentTrackingBehavior = _dbContext.ChangeTracker.QueryTrackingBehavior;
            if (asNoTracking.HasValue && asNoTracking.Value)
            {
                Assert.Equal(QueryTrackingBehavior.NoTracking, currentTrackingBehavior);
            }
            else
            {
                Assert.Equal(QueryTrackingBehavior.TrackAll, currentTrackingBehavior);
            }
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