using AppComponents.CoreLib;
using AppComponents.CoreLib.Repository.Abstraction;
using AppComponents.CoreLib.Repository.EFCore;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CoreLib.Tests
{
    public class RepositoryTests_GetAll : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenCalledOnEmptyDataset()
        {
            //Arrange
            await InitializeAsync(mockItems: new List<MockItem>());
            var repository = GetRepository();

            //Act
            var data = await repository?.GetAll();
            var result = await data.ToListAsync();

            //Assert
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenCalledOnUninitializedDataset()
        {
            //Arrange
            await InitializeAsync(mockItems: null);
            var repository = GetRepository();

            //Act
            var data = await repository?.GetAll();
            var result = await data.ToListAsync();

            //Assert
            Assert.Empty(data);
        }


        [Fact]
        public async Task GetAll_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var data = await repository?.GetAll();
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(TestData.MockItems, result);
        }



        [Fact]
        public async Task GetAll_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var mockItemsWithEvenIds = await repository.GetAll(_queryItemsWithEvenId);
            var result = await mockItemsWithEvenIds.ToListAsync();

            //Assert
            AssertMockItems(TestData.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAll_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act
            var mockItemsWithOddIds = await repository.GetAll(_queryItemsWithOddId);
            var result = await mockItemsWithOddIds.ToListAsync();

            //Assert
            AssertMockItems(TestData.MockItemsWithOddIds, mockItemsWithOddIds); 
        }


        [Fact]
        public async Task GetAll_ReturnsDataAsPerOrder_WhenDifferentSortingIsAppliedToMultipleColumns()
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
            var result = await repository.GetAll(null, false, orderByClause, null);

            //Assert
            AssertMockItems(expectedData, result);
        }

        #region pagination

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public async Task GetAllWithPagination_ReturnsMatchingData_WhenDataIsInTheRange(int pageIndex, int pageSize)
        {
            //Act
            var paginationSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            int takeItems = pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, null, paginationSpec);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }


        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, -2)]
        [InlineData(4, null)]
        [InlineData(null, -1)]
        [InlineData(-1, 2)]
        [InlineData(-2, null)]
        [InlineData(null, 3)]
        public async Task GetAllWithPagination_UsesDefaultValues_WhenInvalidValuesAreUsed(int pageIndex, int pageSize)
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
            var data = await repository.GetAll(null, true, null, paginationSpec);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }



        [Theory]
        [InlineData(1, 30, 20)]
        [InlineData(4, 6, 2)]
        [InlineData(5, 5, 0)]
        public async Task GetAllWithPagination_ReturnsRemainingData_WhenMoreThanExistingDataIsRequested(int pageIndex, int pageSize, int takeItems)
        {
            //Act
            var pageSpec = new Pagination(pageIndex, pageSize);
            InitializeAsync(TestData.MockItemsForPagination);
            var repository = GetRepository();

            int skipItems = (pageIndex - 1) * pageSize;
            var expectedResult = TestData.MockItemsForPagination.Skip(skipItems).Take(takeItems).ToImmutableList();


            //Act
            var data = await repository.GetAll(null, true, null, pageSpec);
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(expectedResult, result);
        }

        #endregion
    }
}