using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests
{
    public class RepositoryTests_GetAllAsync : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync();

            //Assert
           AssertMockItemsEqual(DataList.MockItems, allMockItems);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var mockItemsWithEvenIds = await repository.GetAllAsync(x => x.Id % 2 == 0);

            //Assert
            AssertMockItemsEqual(DataList.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var mockItemsWithOddIds = await repository.GetAllAsync(X => X.Id % 2 != 0);

            //Assert
           AssertMockItemsEqual(DataList.MockItemsWithOddIds, mockItemsWithOddIds); 
        }

        //TODO: Fix issue
        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingDisabled_WhenCalledWithNoTracking()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync(null, true);

            //Assert
            var result = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.NoTracking, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledWithTracking()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync(null, false);

            //Assert
            var result = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledOnDefaultValue()
        {
            //Arrange
            var repository = new Repository<MockItem>(_dbContext);

            //Act
            var allMockItems = await repository.GetAllAsync(null);

            //Assert
            var result = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, result);
        }
    }
}