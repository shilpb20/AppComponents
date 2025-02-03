using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;

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
            var result = await repository.GetAll(null, false, null, null, orderByClause);

            //Assert
            AssertMockItems(expectedData, result);
        }
    }
}