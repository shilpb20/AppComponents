using AppComponents.CoreLib;
using CoreLib.Tests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Tests
{
    public class RepositoryTests_DeleteAsync  : RepositoryTestsBase, IAsyncLifetime
    {
        [Fact]
        public async Task DeleteAsync_DeletesMatchingObject_WhenFound()
        {
            //Arrange
            Repository<MockItem> repository = GetRepository();
            var countBeforeDeletion = _dbContext.MockItems.Count();

            //Act  
            var result = await repository.DeleteAsync(TestData.MockItems.First());
            var deletedObject = await repository.GetAsync(_queryItemWithId1);

            var countAfterDeletion = _dbContext.MockItems.Count();


            //Assert
            AssertMockItem(TestData.MockItems.First(), result);
            Assert.Null(deletedObject);
            Assert.Equal(countBeforeDeletion, countAfterDeletion+1);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsDbConcurrencyException_WhenMatchingObjectNotFound()
        {
            Repository<MockItem> repository = GetRepository();

            //Act  
            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await repository.DeleteAsync(TestData.NewItem);
            });
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenNullIsPassedForDeletion()
        {
            Repository<MockItem> repository = GetRepository();

            //Act  
            var countBeforeDeletion = _dbContext.MockItems.Count();

            var result = await repository.DeleteAsync(null);

            var countAfterDeletion = _dbContext.MockItems.Count();

            //Assert
            Assert.Null(result);
            Assert.Equal(countBeforeDeletion, countAfterDeletion);
        }
    }
}
