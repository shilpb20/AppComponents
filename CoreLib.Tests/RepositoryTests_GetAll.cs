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
            AssertMockItemsEqual(TestData.MockItems, result);
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
            AssertMockItemsEqual(TestData.MockItemsWithEvenIds, mockItemsWithEvenIds);
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
            AssertMockItemsEqual(TestData.MockItemsWithOddIds, mockItemsWithOddIds); 
        }
    }
}