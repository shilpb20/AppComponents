using AppComponents.Repository.EFCore;
using Repository.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq.Expressions;

namespace Repository.Tests
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

        protected async Task AssertTrackingBehaviorForGetAllAsync(
            Repository<MockItem, TestDbContext> repository,
            Expression<Func<MockItem, bool>> filter,
            bool? asNoTracking = null)
        {
            if (asNoTracking.HasValue)
            {
               await repository.GetAllAsync(filter, asNoTracking.Value);
            }
            else
            {
                await repository.GetAsync(filter);
            }
            VerifyTrackingBehavior(asNoTracking);
        }

        protected async Task AssertTrackingBehaviorForGetAsync(
            Repository<MockItem, TestDbContext> repository,
            Expression<Func<MockItem, bool>> filter,
            bool? asNoTracking = null)
        {
            _dbContext.ChangeTracker.Clear();


            if (asNoTracking.HasValue)
            {
                await repository.GetAsync(filter, asNoTracking.Value);
            }
            else
            {
                await repository.GetAsync(filter);
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

        protected Repository<MockItem, TestDbContext> GetRepository()
        {
            if(_dbContext == null)
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