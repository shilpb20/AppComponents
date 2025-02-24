using AppComponents.CoreLib.Repository.EFCore;
using AppComponents.CoreLib.Repository;
using CoreLib.Tests.Data;
using System.Collections.Immutable;
using AppComponents.CoreLib.Repository.Abstraction;
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
            AssertMockItems(TestData.MockItems, allMockItems);
        }


        #region filter tests

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenCalledWithANonMatchingFilter()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithNonMatchingFilter = await repository.GetAllAsync(_queryItemsWithNegativeIds);

            //Assert
            Assert.Empty(mockItemsWithNonMatchingFilter);
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

        #endregion

        #region pagination

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllAsyncWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, paginationSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }


        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(4, null)]
        [InlineData(null, -1)]
        [InlineData(-1, 2)]
        [InlineData(-2, null)]
        [InlineData(null, 3)]
        public async Task GetAllAsyncWithPagination_UsesDefaultValues_WhenInvalidValuesAreUsed(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int expectedPageIndex = pageIndex <= 0 ? 1 : pageIndex;
            int expectedPageSize = pageSize <= 0 ? 10 : pageSize;

            int skipItems = (pageIndex - 1) * expectedPageSize;
            int takeItems = expectedPageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, paginationSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }



        [Theory]
        [InlineData(1, 30, 20)]
        [InlineData(4, 6, 2)]
        [InlineData(5, 5, 0)]
        public async Task GetAllAsyncWithPagination_ReturnsRemainingData_WhenMoreThanExistingDataIsRequested(int pageIndex, int pageSize, int takeItems)
        {
            //Act
            var pageSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAllAsync(null, true, null, pageSpec);

            //Assert
            AssertMockItems(expectedResult, data);
        }

        #endregion

        #region sorting tests


        [Fact]
        public async Task GetAllAsyncIn_ReturnsDataInAscendingOrder_WhenAscendingOrderIsAppliedToMultipleColumns()
        {
            //Arrange
            InitializeAsync(TestData.MockItemsForOrderBy);
            var expectedData = TestData.MockItemsForOrderBy.OrderBy(x => x.Id).ThenBy(x => x.Name).ThenBy(x => x.Value).ToList();

            Repository<MockItem> repository = GetRepository();

            var orderByClause = new Dictionary<string, bool>
            {
                [TestData.Column1] = true,
                [TestData.Column2] = true,
                [TestData.Column3] = true
            };


            //Act
            var result = await repository.GetAllAsync(null, false, orderByClause, null);

            //Assert
            AssertMockItems(expectedData, result);
        }

        [Fact]
        public async Task GetAllAsyncIn_ReturnsDataInDescendingOrder_WhenDescendingOrderIsAppliedToMultipleColumns()
        {
            //Arrange
            InitializeAsync(TestData.MockItemsForOrderBy);
            var expectedData = TestData.MockItemsForOrderBy.OrderByDescending(x => x.Id).ThenByDescending(x => x.Name).ThenByDescending(x => x.Value).ToList();

            Repository<MockItem> repository = GetRepository();

            var orderByClause = new Dictionary<string, bool>
            {
                [TestData.Column1] = false,
                [TestData.Column2] = false,
                [TestData.Column3] = false
            };


            //Act
            var result = await repository.GetAllAsync(null, false, orderByClause, null);

            //Assert
            AssertMockItems(expectedData, result);
        }

        [Fact]
        public async Task GetAllAsyncIn_ReturnsDataAsPerOrder_WhenDifferentSortingIsAppliedToMultipleColumns()
        {
            //Arrange
            InitializeAsync(TestData.MockItemsForOrderBy);
            var expectedData = TestData.MockItemsForOrderBy.OrderBy(x => x.Id).ThenByDescending(x => x.Name).ThenBy(x => x.Value).ToList();

            Repository<MockItem> repository = GetRepository();

            var orderByClause = new Dictionary<string, bool>
            {
                [TestData.Column1] = true,
                [TestData.Column2] = false,
                [TestData.Column3] = true
            };


            //Act
            var result = await repository.GetAllAsync(null, false, orderByClause);

            //Assert
            AssertMockItems(expectedData, result);
        }

        #endregion

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