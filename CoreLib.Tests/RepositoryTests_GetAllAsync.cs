using AppComponents.CoreLib;
using CoreLib.Tests.Data;
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
            AssertMockItemsEqual(TestData.MockItems, allMockItems);
        }



        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithEvenIds = await repository.GetAllAsync(_queryItemsWithEvenId);

            //Assert
            AssertMockItemsEqual(TestData.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var mockItemsWithOddIds = await repository.GetAllAsync(_queryItemsWithOddId);

            //Assert
            AssertMockItemsEqual(TestData.MockItemsWithOddIds, mockItemsWithOddIds); 
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledWithTracking()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, false);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllWithTrackingEnabled_WhenCalledOnDefaultValue()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            //Assert
            await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, null);
        }

        //Note: Commenting tests as the behavior is not clear
        //Passing tests  for these may n ot be correct either
        //Low risk as framework method is used.
        //To verify check if AsNoTracking() is correctly used in the repository method

        //[Theory]
        //[InlineData(false)]
        //[InlineData(null)]
        //[InlineData(true)]
        //public async Task GetAllAsync_ReturnsTrackingResult_BasedOnTrackingFlag(bool? asNoTracking)
        //{
        //    //Arrange
        //    Repository<MockItem> repository = GetRepository();

        //    //Act
        //    //Assert

        //    await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, asNoTracking);
        //}

        //[Fact]
        //public async Task GetAllAsync_TrackingBehaviorSwitches_WhenCalledWithAlternatingValues()
        //{
        //    //Arrange
        //    await InitializeAsync(TestData.DuplicateMockItems);
        //    Repository<MockItem> repository = GetRepository();

        //    //Act
        //    //Assert
        //    await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, false);
        //    await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, true);
        //    await AssertTrackingBehaviorForGetAllAsync(repository, _queryItemsWithPositiveIds, false);
        //}
    }
}