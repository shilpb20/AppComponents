using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_DeleteAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task AddAsync_ReturnsNull_WhenMatchingObjectNotFound()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();
            var countBeforeDeletion = _dbContext.MockItems.Count();

            //Act  
            var result = await repository.DeleteAsync(1);
            var deletedObject = await repository.GetAsync(_queryItemWithId1);

            var countAfterDeletion = _dbContext.MockItems.Count();


            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
            Assert.Null(deletedObject);
            Assert.Equal(countBeforeDeletion, countAfterDeletion+1);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndReturnsMatchingObject_WhenMatchingObjectFound()
        {
            Repository<MockItem> repository = GetRepository();

            //Act  
            var result = await repository.DeleteAsync(0);

            //Assert
            Assert.Null(result);
        }
    }
}
