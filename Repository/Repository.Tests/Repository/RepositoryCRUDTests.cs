using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using Repository.Tests.Repository.TestContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Tests.Repository
{
    public class RepositoryCRUDTests : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task AddAsync_ReturnsNewlyAddedObject_WhenValidObjectIsAdded()
        {
            //Arrange
            var repository = GetRepository();

            //Act  
            var newItem = TestData.NewItem;
            var countBeforeAddition = _dbContext.MockItems.Count();
            var result = await repository.AddAsync(newItem);

            var newlyAddedObject = await repository.GetAsync(_queryNewItemByName);
            var countAfterAddition = _dbContext.MockItems.Count();

            //Assert
            AssertMockItem(result, newlyAddedObject);
            AssertMockItem(result, newItem);

            Assert.Equal(countBeforeAddition, countAfterAddition - 1);
        }

        [Fact]
        public async Task GetAsync_ReturnsMatchingItem_WhenSingleMatchingDataFound()
        {
            //Arrange
            var repository = GetRepository();
            //Act
            var result = await repository.GetAsync(_queryItemWithId1);

            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            var repository = GetRepository();

            //Act
            var allMockItems = await repository?.GetAllAsync();

            //Assert
            AssertMockItems(TestData.MockItems, allMockItems);
        }

        [Fact]
        public async Task GetAll_ReturnsAllItems_WhenCalled()
        {
            //Arrange
            var repository = GetRepository();

            //Act
            var data = await repository?.GetAll();
            var result = await data.ToListAsync();

            //Assert
            AssertMockItems(TestData.MockItems, result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMatchingObject_WhenFound()
        {
            //Arrange
            var repository = GetRepository();
            var countBeforeDeletion = _dbContext.MockItems.Count();

            //Act  
            var result = await repository.DeleteAsync(TestData.MockItems.First());
            var deletedObject = await repository.GetAsync(_queryItemWithId1);

            var countAfterDeletion = _dbContext.MockItems.Count();


            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
            Assert.Null(deletedObject);
            Assert.Equal(countBeforeDeletion, countAfterDeletion + 1);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedValue_OnValidUpdate()
        {
            //Arrange
            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act
            //Assert
            var updateObject = await repository.GetAsync(_queryItemForUpdateData);
            AssertMockItem(TestData.DuplicateMockItems.Last(), updateObject);
            updateObject.Name = TestData.UpdateItem.Name;
            updateObject.Value = TestData.UpdateItem.Value;

            var result = await repository.UpdateAsync(updateObject);
            AssertMockItem(TestData.UpdateItem, result);
        }
    }
}
