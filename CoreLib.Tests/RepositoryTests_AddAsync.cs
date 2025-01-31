using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_AddAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task AddAsync_ReturnsNewlyAddedObject_WhenValidObjectIsAdded()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

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
        public async Task AddAsync_ReturnsNull_WhenNullObjectIsAdded()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();

            //Act  
            var newItem = TestData.NewItem;
            var result = await repository.AddAsync(null);

            //Assert
            Assert.Null(result);
        }
    }
}
