using AppComponents.Repository.EFCore;
using Repository.Tests.Repository.TestContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Tests.Repository
{
    public class RepositoryFilterAndSortTests : RepositoryTestsBase, IAsyncLifetime
    {

        #region filter tests

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenCalledWithANonMatchingFilter()
        {
            //Arrange
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithNonMatchingFilter = await repository.GetAllAsync(_queryItemsWithNegativeIds);

            //Assert
            Assert.Empty(mockItemsWithNonMatchingFilter);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            var repository = GetRepository();

            //Act
            IEnumerable<MockItem> mockItemsWithEvenIds = await repository.GetAllAsync(_queryItemsWithEvenId);

            //Assert
            AssertMockItems(TestData.MockItemsWithEvenIds, mockItemsWithEvenIds);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_OddIds()
        {
            //Arrange
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act
            var mockItemsWithOddIds = await repository.GetAllAsync(_queryItemsWithOddId);

            //Assert
            AssertMockItems(TestData.MockItemsWithOddIds, mockItemsWithOddIds);
        }

        #endregion

        #region sorting tests


        [Fact]
        public async Task GetAllAsyncIn_ReturnsDataInAscendingOrder_WhenAscendingOrderIsAppliedToMultipleColumns()
        {
            //Arrange
            InitializeAsync(TestData.MockItemsForOrderBy);
            var expectedData = TestData.MockItemsForOrderBy.OrderBy(x => x.Id).ThenBy(x => x.Name).ThenBy(x => x.Value).ToList();

            Repository<MockItem, TestDbContext> repository = GetRepository();

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

            Repository<MockItem, TestDbContext> repository = GetRepository();

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

            Repository<MockItem, TestDbContext> repository = GetRepository();

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
    }
}
