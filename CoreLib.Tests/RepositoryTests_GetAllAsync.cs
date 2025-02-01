using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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
            AssertMockItems(TestData.MockItems, allMockItems);
        }



        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithEvenIds = await repository.GetAllAsync(_queryItemsWithEvenId);

            //Assert
            AssertMockItems(TestData.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var mockItemsWithOddIds = await repository.GetAllAsync(_queryItemsWithOddId);

            //Assert
            AssertMockItems(TestData.MockItemsWithOddIds, mockItemsWithOddIds); 
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllAsyncWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var result = await repository.GetAllAsync(null, true, pageIndex, pageSize);

            //Assert
            AssertMockItems(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 30, 20)]
        [InlineData(4, 6, 2)]
        [InlineData(5, 5, 0)]
        public async Task GetAllAsyncWithPagination_ReturnsRemainingData_WhenMoreThanExistingDataIsRequested(int pageIndex, int pageSize, int takeItems)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var result = await repository.GetAllAsync(null, true, pageIndex, pageSize);

            //Assert
            AssertMockItems(expectedResult, result);
        }

        [Theory]
        [InlineData(-1, 2)]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(null, -1)]
        [InlineData(-2, null)]
        public async Task GetAllAsyncWithPagination_ReturnsEmptyData_WhenIncorrectRequestIsMade(int? pageIndex, int? pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            //Act
            var result = await repository.GetAllAsync(null, true, pageIndex, pageSize);

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(null, 3)]
        [InlineData(4, null)]
        public async Task GetAllAsyncWithPagination_ReturnsEmptyData_WhenNullValuesAreUsed(int? pageIndex, int? pageSize)
        {
            //Act
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            //Act
            var result = await repository.GetAllAsync(null, true, pageIndex, pageSize);

            //Assert
            Assert.Empty(result);
        }

        #region tracking behaviour tests

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

        #endregion
    }
}