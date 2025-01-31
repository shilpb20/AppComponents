using AppComponents.CoreLib;
using CoreLib.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Tests
{
    public class RepositoryTests_GetAllAsync : RepositoryTestsBase, IAsyncLifetime
    {   

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenCalledOnEmptyDataset()
        {
            //Arrange
            await InitializeAsync(mockItems: new List<MockItem>());
            var repository = GetRepository();

            //Act
            var data = await repository?.GetAllAsync();


            //Assert
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenCalledOnUninitializedDataset()
        {
            //Arrange
            await InitializeAsync(mockItems: null);
            var repository = GetRepository();

            //Act
            var data = await repository?.GetAllAsync();


            //Assert
            Assert.Empty(data);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var allMockItems = await repository?.GetAllAsync();

            //Assert
            AssertMockItemsEqual(DataList.MockItems, allMockItems);
        }



        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithEvenIds = await repository.GetAllAsync(x => x.Id % 2 == 0);

            //Assert
            AssertMockItemsEqual(DataList.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

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
            Repository<MockItem> repository = GetRepository();

            //Act
            await repository?.GetAllAsync(null, true);

            //Assert
            var result = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.NoTracking, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledWithTracking()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            await repository.GetAllAsync(null, false);

            //Assert
            var result = _dbContext.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledOnDefaultValue()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            await repository.GetAllAsync(null);

            //Assert
            var result = _dbContext?.ChangeTracker.QueryTrackingBehavior;
            Assert.Equal(QueryTrackingBehavior.TrackAll, result);
        }
    }
}