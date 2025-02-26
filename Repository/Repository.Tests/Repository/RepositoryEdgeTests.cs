using AppComponents.Repository.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Tests.Repository.TestContext;

namespace Repository.Tests.Repository
{
    public class RepositoryEdgeTests : RepositoryTestsBase, IAsyncLifetime
    {
        #region add

        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException_WhenNullObjectIsAdded()
        {
            //Arrange
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act  
            //Assert
            var newItem = TestData.NewItem;
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await repository.AddAsync(null);
            });
        }

        #endregion

        #region update

        [Fact]
        public async Task UpdateAsync_ThrowsInvalidOperationException_OnUpdateNull()
        {
            //Arrange
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act  
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await repository.UpdateAsync(null);
            });
        }

        #endregion

        #region delete

        [Fact]
        public async Task DeleteAsync_ThrowsDbConcurrencyException_WhenMatchingObjectNotFound()
        {
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act  
            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await repository.DeleteAsync(TestData.NewItem);
            });
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNullArgumntException_WhenNullIsPassedForDeletion()
        {
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act  
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await repository.DeleteAsync(null);
            });
        }

        #endregion

        #region get-all-async

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

        #endregion

        #region get-async

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenMatchingDataNotFound()
        {
            //Arrange
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWithId0);

            //Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task GetAsync_ReturnsFirstMatchingItem_WhenMultipleMatchingDataFound()
        {
            //Arrange
            var expectedResult = TestData.FirstDuplicateItem;

            await InitializeAsync(TestData.DuplicateMockItems);
            Repository<MockItem, TestDbContext> repository = GetRepository();

            //Act
            var result = await repository.GetAsync(_queryItemWitDuplicateName);

            //Assert
            AssertMockItem(expectedResult, result);
        }

        #endregion

        #region get-all

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
        public async Task GetAll_ReturnsAllItemsWithMatchingCondition_WhenCalledWithAMatchCondition_EvenIds()
        {
            //Arrange
            var repository = GetRepository();

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
            var repository = GetRepository();

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

            var repository = GetRepository();

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



        #endregion
    }
}
