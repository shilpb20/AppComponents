using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests
{
    public class RepositoryTests
    {
        private TestDbContext _dbContext;

        private async Task InitializeData()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            _dbContext = new TestDbContext(options);
            await _dbContext.MockItems.AddRangeAsync(DataList.MockItems);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.Equal(DataList.MockItems.Count, allMockItems.Count());

            int i = 0;
            foreach (var item in allMockItems)
            {
                Assert.Equal(DataList.MockItems[i].Id, item.Id);
                Assert.Equal(DataList.MockItems[i].Name, item.Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var mockItemsWithEvenIds = await repository.GetAllAsync(x => x.Id % 2 == 0);

            //Assert
            Assert.NotNull(mockItemsWithEvenIds);
            Assert.Equal(DataList.MockItemsWithEvenIds.Count, mockItemsWithEvenIds.Count());

            int i = 0;
            foreach (var item in mockItemsWithEvenIds)
            {
                Assert.Equal(DataList.MockItemsWithEvenIds[i].Id, item.Id);
                Assert.Equal(DataList.MockItemsWithEvenIds[i].Name, item.Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var mockItemsWithOddIds = await repository.GetAllAsync(X => X.Id % 2 != 0);

            //Assert
            Assert.NotNull(mockItemsWithOddIds);
            Assert.Equal(DataList.MockItemsWithOddIds.Count, mockItemsWithOddIds.Count());

            int i = 0;
            foreach (var item in mockItemsWithOddIds)
            {
                Assert.Equal(DataList.MockItemsWithOddIds[i].Id, item.Id);
                Assert.Equal(DataList.MockItemsWithOddIds[i].Name, item.Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingDisabled_WhenCalledWithNoTracking()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            _dbContext.ChangeTracker.Clear();
            var allMockItems = await repository.GetAllAsync(null, true);
            var isTracked = _dbContext.ChangeTracker.Entries<MockItem>().Any();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.False(isTracked);

            Assert.Equal(DataList.MockItems.Count, allMockItems.Count());

            int i = 0;
            foreach (var item in allMockItems)
            {
                Assert.Equal(DataList.MockItems[i].Id, item.Id);
                Assert.Equal(DataList.MockItems[i].Name, item.Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledWithTracking()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act
            _dbContext.ChangeTracker.Clear();

            var allMockItems = await repository.GetAllAsync(null, false);
            var isTracked = _dbContext.ChangeTracker.Entries<MockItem>().Any();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.True(isTracked);

            Assert.Equal(DataList.MockItems.Count, allMockItems.Count());

            int i = 0;
            foreach (var item in allMockItems)
            {
                Assert.Equal(DataList.MockItems[i].Id, item.Id);
                Assert.Equal(DataList.MockItems[i].Name, item.Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledOnDefaultValue()
        {
            //Arrange
            await InitializeData();

            var repository = new Repository<MockItem>(_dbContext);

            //Act

            _dbContext.ChangeTracker.Clear();
            var allMockItems = await repository.GetAllAsync(null, false);
            var isTracked = _dbContext.ChangeTracker.Entries<MockItem>().Any();

            //Assert
            Assert.NotNull(allMockItems);
            Assert.True(isTracked);

            Assert.Equal(DataList.MockItems.Count, allMockItems.Count());

            int i = 0;
            foreach (var item in allMockItems)
            {
                Assert.Equal(DataList.MockItems[i].Id, item.Id);
                Assert.Equal(DataList.MockItems[i].Name, item.Name);

                i++;
            }
        }
    }
}